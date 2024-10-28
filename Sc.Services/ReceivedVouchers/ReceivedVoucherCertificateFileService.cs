using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FileStorageNetCore.Api;
using Infrastructure.AppSettings;
using Infrastructure.FileManagementPackages.FileConverters;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Suppliers;

namespace Sc.Services.ReceivedVouchers
{
    public class ReceivedVoucherCertificateFileService
    {
        private readonly BlobStorageService blobStorageService;
        private readonly FileConverterService fileConverterService;

        public ReceivedVoucherCertificateFileService(
            BlobStorageService blobStorageService,
            FileConverterService fileConverterService
            )
        {
            this.blobStorageService = blobStorageService;
            this.fileConverterService = fileConverterService;
        }

        public async Task<ReceivedVoucherCertificateFileDto> GenerateCertificateFile(ReceivedVoucher receivedVoucher, ReceivedVoucherCertificateDto certificateDto)
        {
            var certificateTemplateBytes = File.ReadAllBytes(AppSettingsProvider.CertificateTemplatePath);
            using var templateStream = new MemoryStream(0);
            templateStream.Write(certificateTemplateBytes, 0, certificateTemplateBytes.Length);
            using var certificateTemplate = WordprocessingDocument.Open(templateStream, true);

            var contentControls = certificateTemplate.MainDocumentPart.RootElement.Descendants<SdtElement>().ToList();

            foreach (var control in contentControls)
            {
                CheckControl(control, receivedVoucher, certificateDto);
            }

            certificateTemplate.Dispose();

            var pdfStream = fileConverterService.OpenXmlToPdf(templateStream);

            var attachedFile = await blobStorageService.Post(pdfStream.ToArray(), $"{receivedVoucher.ContractNumber}_{certificateDto.Offering.Code}.pdf", "application/pdf");

            var result = new ReceivedVoucherCertificateFileDto
            {
                DbId = attachedFile.DbId,
                Hash = attachedFile.Hash,
                Key = attachedFile.Key,
                MimeType = attachedFile.MimeType,
                Name = attachedFile.Name,
                Size = attachedFile.Size
            };

            return result;
        }

        private void CheckControl(SdtElement control, ReceivedVoucher receivedVoucher, ReceivedVoucherCertificateDto certificateDto)
        {
            var tagName = control.SdtProperties.GetFirstChild<Tag>()?.Val?.ToString()?.Trim()?.TrimEnd(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            if (!string.IsNullOrWhiteSpace(tagName))
            {
                switch (tagName)
                {
                    case "CONTRACT_DATE":
                        SetTextInputValue(control, receivedVoucher.ContractDate.ToString("dd.MM.yyyy"));
                        break;
                    case "CONTRACT_NUMBER":
                        SetTextInputValue(control, receivedVoucher.ContractNumber);
                        break;
                    case "SUPPLIER":
                        SetTextInputValue(control, certificateDto.Supplier.Type == SupplierType.Complex
                            ? certificateDto.Supplier.Complex.Name
                            : ($"{certificateDto.Supplier.Institution.Name} {(certificateDto.Supplier.Institution.Id != certificateDto.Supplier.Institution.RootId ? $"({certificateDto.Supplier.Institution.Root.ShortName})" : string.Empty)}"));
                        break;
                    case "SUPPLIER_OFFERING":
                        SetTextInputValue(control, $"{certificateDto.Offering.Code} - {certificateDto.Offering.Name}");
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetTextInputValue(SdtElement control, string value)
        {
            control.Descendants<Text>().First().Text = value;
        }
    }
}
