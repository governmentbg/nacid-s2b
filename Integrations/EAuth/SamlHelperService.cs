using Infrastructure.AppSettings;
using Infrastructure.Helpers;
using Integrations.EAuth.Dtos;
using Integrations.EAuth.Enums;
using Microsoft.AspNetCore.Http;
using SAML2;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Integrations.EAuth
{
    public class SamlHelperService
    {
        private readonly CertificateService certificateService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SamlHelperService(
            CertificateService certificateService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.certificateService = certificateService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public XmlDocument GenerateKEPAuthnRequest()
        {
            var req = new Saml20AuthnRequest();
            req.ProtocolBinding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";
            req.Request.ProviderName = AppSettingsProvider.EAuthConfiguration.ProviderName;
            req.Request.Issuer.SPProvidedID = AppSettingsProvider.EAuthConfiguration.ProviderId;
            req.Request.Issuer.Value = AppSettingsProvider.EAuthConfiguration.Url + "api/EAuth/Metadata";
            req.Request.IssueInstant = DateTime.Now;
            req.Request.Destination = AppSettingsProvider.EAuthConfiguration.RequestUrl;
            req.Request.ForceAuthn = false;
            req.Request.IsPassive = false;
            req.Request.AssertionConsumerServiceUrl = AppSettingsProvider.EAuthConfiguration.Url + "api/EAuth/Login";

            req.Request.Extensions = new SAML2.Schema.Protocol.Extensions
            {
                Any = new XmlElement[] { GetExtensions() }
            };

            var namespaces = new XmlSerializerNamespaces(new XmlSerializerNamespaces());
            namespaces.Add("egovbga", "urn:bg:egov:eauth:2.0:saml:ext");
            namespaces.Add("saml2p", "urn:oasis:names:tc:SAML:2.0:protocol");
            namespaces.Add("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
            namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");

            var resp = SamlSerialization.Serialize(req.Request, namespaces);

            if (resp.FirstChild is XmlDeclaration)
            {
                resp.RemoveChild(resp.FirstChild);
            }

            return resp;
        }

        public string SignXmlDocument(XmlDocument xmlDocument, string certificateName, string certPassword)
        {
            var cert = certificateService.GetFromFile(certificateName, certPassword);

            var signedDocument = SignDocument(xmlDocument, cert);
            var signedDocumentString = SignedDocumentToString(signedDocument);
            bool verified = VerifySignature(signedDocumentString, "Signature");

            return signedDocumentString;
        }

        public string ConstuctRedirectUrl(SamlResponseDto dto)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(dto.SAMLResponse))
            {
                var decodedResponseStream = new MemoryStream(Convert.FromBase64String(dto.SAMLResponse));
                var eAuthLoginDataDto = ParseEAuthResponse(decodedResponseStream);

                var name = !string.IsNullOrEmpty(eAuthLoginDataDto.Name) ? eAuthLoginDataDto.Name : null;
                url = AppSettingsProvider.EAuthConfiguration.Url + "eAuthResponse?responseStatus=" + eAuthLoginDataDto.ResponseStatus + "&name=" + name;
            }
            else
            {
                url = AppSettingsProvider.EAuthConfiguration.Url + "eAuthResponse?responseStatus=" + EAuthResponseStatus.InvalidResponseXML;
            }

            return url;
        }

        public string GenerateXmlMetadata(string fileName, string certificatePass)
        {
            var cert = certificateService.GetFromFile(fileName, certificatePass);
            var xmlResponse = string.Format(@"<EntityDescriptor xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""urn:oasis:names:tc:SAML:2.0:metadata"" entityID=""https://nacid.bg"" cacheDuration=""PT5M"">
	            <SPSSODescriptor protocolSupportEnumeration=""urn:oasis:names:tc:SAML:2.0:protocol"">
		            <KeyDescriptor use=""encryption"">
			            <KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">
			            <X509Data>
				            <X509Certificate>{0}</X509Certificate>
			            </X509Data>
			            </KeyInfo>
		            </KeyDescriptor>
	            </SPSSODescriptor>
            </EntityDescriptor>", Convert.ToBase64String(cert.RawData));
            return xmlResponse;
        }

        private XmlElement GetExtensions()
        {
            var doc = new XmlDocument();
            var requestedService = doc.CreateElement("egovbga", "RequestedService", "urn:bg:egov:eauth:2.0:saml:ext");
            doc.AppendChild(requestedService);

            var service = doc.CreateElement("egovbga", "Service", "urn:bg:egov:eauth:2.0:saml:ext");
            service.InnerText = AppSettingsProvider.EAuthConfiguration.CertificateId;
            requestedService.AppendChild(service);

            var provider = doc.CreateElement("egovbga", "Provider", "urn:bg:egov:eauth:2.0:saml:ext");
            provider.InnerText = AppSettingsProvider.EAuthConfiguration.ExtProviderId;
            requestedService.AppendChild(provider);

            var levelOfAssurance = doc.CreateElement("egovbga", "LevelOfAssurance", "urn:bg:egov:eauth:2.0:saml:ext");
            levelOfAssurance.InnerText = AppSettingsProvider.EAuthConfiguration.SecurityLevel;
            requestedService.AppendChild(levelOfAssurance);

            return doc.DocumentElement;
        }

        private XmlDocument SignDocument(XmlDocument doc, X509Certificate2 cert)
        {
            SignedXml xml = new SignedXml(doc)
            {
                SignedInfo =
                {
                    CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl,
                    SignatureMethod = SignedXml.XmlDsigRSASHA1Url
                },
                SigningKey = cert.GetRSAPrivateKey()
            };

            var reference = new Reference();
            reference.Uri = string.Empty;
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());
            xml.AddReference(reference);

            xml.KeyInfo = new KeyInfo();
            var keyInfoData = new KeyInfoX509Data(cert);
            keyInfoData.AddIssuerSerial(cert.Issuer, cert.SerialNumber);
            keyInfoData.AddSubjectName(cert.Subject);
            xml.KeyInfo.AddClause(keyInfoData);
            xml.ComputeSignature();

            var signatureXml = xml.GetXml();

            if (doc.DocumentElement != null)
            {
                XmlNodeList elementsByTagName = doc.DocumentElement.GetElementsByTagName("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                XmlNode parentNode = elementsByTagName[0].ParentNode;
                if (parentNode != null)
                {
                    parentNode.InsertAfter(doc.ImportNode(signatureXml, true), elementsByTagName[0]);
                }
            }

            return doc;
        }

        private string SignedDocumentToString(XmlDocument doc)
        {
            var sb = new StringBuilder();
            using (TextWriter sw = new StringWriter(sb))
            {
                using (XmlTextWriter xmltw = new XmlTextWriter(sw))
                {
                    doc.WriteTo(xmltw);
                }
            }
            return sb.ToString();
        }

        private bool VerifySignature(string xmlString, string signatureTag)
        {
            return VerifySigningInternal(xmlString, signatureTag, false, null, false);
        }

        private bool VerifySigningInternal(String xmlString, string signatureTag, bool verifyCertificate, string certificateThumbprint, bool validateCertificateExpirationDate)
        {
            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();

            // Load the passed XML file into the document. 
            xmlDocument.LoadXml(xmlString);

            // Create a new SignedXml object and pass it y
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Find the "Signature" node and create a new 
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(signatureTag);

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            if (verifyCertificate)
            {
                KeyInfoX509Data x509data = signedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>().First();
                X509Certificate2 signingCertificate = (X509Certificate2)x509data.Certificates[0];

                if (signingCertificate.Thumbprint.ToLower().Trim() != certificateThumbprint.ToLower().Trim())
                {
                    return false;
                }

                if (validateCertificateExpirationDate)
                {
                    if (DateTime.Now < signingCertificate.NotBefore || DateTime.Now > signingCertificate.NotAfter)
                    {
                        return false;
                    }
                }
            }

            return signedXml.CheckSignature();
        }

        private EAuthLoginDataDto ParseEAuthResponse(Stream SamlResponse)
        {
            var eAuthLoginDataDto = new EAuthLoginDataDto();
            if (SamlResponse == null)
            {
                throw new ArgumentNullException("SamlResponse");
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(SamlResponse);
            }
            catch
            {
                eAuthLoginDataDto.ResponseStatus = EAuthResponseStatus.InvalidResponseXML;
                return eAuthLoginDataDto;
            }

            var responseElement = doc.DocumentElement;
            DecryptResponse(doc);

            var samlNS = new XmlNamespaceManager(doc.NameTable);
            samlNS.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
            samlNS.AddNamespace("saml2p", "urn:oasis:names:tc:SAML:2.0:protocol");
            samlNS.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            var statusCode = responseElement.SelectSingleNode("//saml2p:Status/saml2p:StatusCode", samlNS);
            if (statusCode != null)
            {
                var statusCodeValue = statusCode.Attributes["Value"].Value;
                var innerStatusCode = statusCode.SelectSingleNode("saml2p:StatusCode", samlNS);
                if (innerStatusCode != null)
                {
                    statusCodeValue = innerStatusCode.Attributes["Value"].Value;
                }
                var statusMessage = responseElement.SelectSingleNode("//saml2p:Status/saml2p:StatusMessage", samlNS);
                eAuthLoginDataDto.ResponseStatusMessage = statusMessage != null ? HttpUtility.HtmlDecode(statusMessage.InnerText) : string.Empty;
                eAuthLoginDataDto.ResponseStatus = GetResponseStatusFromCode(statusCodeValue, eAuthLoginDataDto.ResponseStatusMessage);
            }

            if (eAuthLoginDataDto.ResponseStatus != EAuthResponseStatus.Success)
            {
                return eAuthLoginDataDto;
            }

            var attributes = responseElement.SelectSingleNode("//saml2:EncryptedAssertion/saml2:Assertion/saml2:AttributeStatement", samlNS);
            if (attributes != null)
            {
                var peronIdentifier = attributes.SelectSingleNode("saml2:Attribute[@Name='urn:egov:bg:eauth:2.0:attributes:personIdentifier']/saml2:AttributeValue", samlNS);
                if (peronIdentifier != null) eAuthLoginDataDto.Egn = peronIdentifier.InnerText;

                var name = attributes.SelectSingleNode("saml2:Attribute[@Name='urn:egov:bg:eauth:2.0:attributes:personName']/saml2:AttributeValue", samlNS);
                if (name != null) eAuthLoginDataDto.Name = name.InnerText;
            }

            return eAuthLoginDataDto;
        }

        private void DecryptResponse(XmlDocument xml)
        {
            var samlNS = new XmlNamespaceManager(xml.NameTable);
            samlNS.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
            samlNS.AddNamespace("saml2p", "urn:oasis:names:tc:SAML:2.0:protocol");
            samlNS.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            samlNS.AddNamespace("xenc", "http://www.w3.org/2001/04/xmlenc#");
            var encryptedNode = xml.SelectSingleNode("//saml2:EncryptedAssertion/xenc:EncryptedData", samlNS) as XmlElement;

            var encryptedXml = new EncryptedXml(xml);
            var encryptedData = new EncryptedData();
            encryptedData.LoadXml(encryptedNode);

            var cert = certificateService.GetFromFile(AppSettingsProvider.EAuthConfiguration.RequestSignCertificate, AppSettingsProvider.EAuthConfiguration.RequestSignCertificatePass);

            var privateKey = cert.GetRSAPrivateKey();
            var cipherNode = xml.SelectSingleNode("//saml2:EncryptedAssertion/xenc:EncryptedData/ds:KeyInfo/xenc:EncryptedKey/xenc:CipherData/xenc:CipherValue", samlNS);
            var cipher = cipherNode.InnerText;
            var cipherBytes = Convert.FromBase64String(cipher);
            byte[] decryptedPrivateKey = null;
            if (privateKey != null)
            {
                decryptedPrivateKey = privateKey.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA1);
            }

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.None;
            aes.Key = decryptedPrivateKey;

            var decryptedData = encryptedXml.DecryptData(encryptedData, aes);
            encryptedXml.ReplaceData(encryptedNode, decryptedData);
        }

        private EAuthResponseStatus GetResponseStatusFromCode(string statusCode, string statusMessage)
        {
            switch (statusCode)
            {
                case "urn:oasis:names:tc:SAML:2.0:status:AuthnFailed":
                    if (statusMessage.Trim().ToLower() == "отказан от потребител")
                        return EAuthResponseStatus.CanceledByUser;
                    else if (statusMessage.Trim().ToLower() == "not_detected_qes")
                        return EAuthResponseStatus.NotDetectedQES;
                    else
                        return EAuthResponseStatus.AuthenticationFailed;
                case "urn:oasis:names:tc:SAML:2.0:status:Success":
                    return EAuthResponseStatus.Success;
            }

            return EAuthResponseStatus.AuthenticationFailed;
        }
    }
}
