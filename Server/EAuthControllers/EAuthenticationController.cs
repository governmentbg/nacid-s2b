using Infrastructure.AppSettings;
using Integrations.EAuth;
using Integrations.EAuth.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Server.EAuthControllers
{
    [ApiController]
    [Route("api/EAuth")]
    public class EAuthenticationController : ControllerBase
    {
        private readonly SamlHelperService samlHelperService;

        public EAuthenticationController(SamlHelperService samlHelperService)
        {
            this.samlHelperService = samlHelperService;
        }

        [HttpGet]
        public ActionResult<SamlRequestDto> EAuth()
        {
            var authnRequest = samlHelperService.GenerateKEPAuthnRequest();

            var signedAuthnRequest = samlHelperService.SignXmlDocument(authnRequest, AppSettingsProvider.EAuthConfiguration.RequestSignCertificate, AppSettingsProvider.EAuthConfiguration.RequestSignCertificatePass);
            var base64AuthnRequest = Convert.ToBase64String(Encoding.UTF8.GetBytes(signedAuthnRequest));

            var samlRequest = new SamlRequestDto();
            samlRequest.PostUrl = AppSettingsProvider.EAuthConfiguration.RequestUrl;
            samlRequest.KeyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("SAMLRequest", base64AuthnRequest),
                new KeyValuePair<string, string>("RelayState", string.Empty)
            };

            return Ok(samlRequest);
        }

        [HttpPost("Login")]
        public RedirectResult EAuthLogin([FromForm] SamlResponseDto dto)
        {
            return Redirect(samlHelperService.ConstuctRedirectUrl(dto));
        }

        [HttpGet("Metadata")]
        public FileStreamResult GetEAuthMetadata()
        {
            var xmlResponse = samlHelperService.GenerateXmlMetadata(AppSettingsProvider.EAuthConfiguration.RequestSignCertificate, AppSettingsProvider.EAuthConfiguration.RequestSignCertificatePass);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(xmlResponse);
            writer.Flush();
            stream.Position = 0;

            return new FileStreamResult(stream, "text/xml");
        }
    }
}
