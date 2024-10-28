using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes.Integrations;
using Integrations.AgencyRegixIntegration.Dtos;
using RegiXConsumer.Models;
using RegiXConsumer.Services;
using System.Xml;
using System.Xml.Serialization;
using static RegiXServiceReference.RegiXEntryPointV2Client;

namespace Integrations.AgencyRegixIntegration
{
    public class AgencyRegixIntegrationService
    {
        private readonly IRegiXService regiXService;
        private readonly DomainValidatorService domainValidatorService;

        public AgencyRegixIntegrationService(
            IRegiXService regiXService,
            DomainValidatorService domainValidatorService
        )
        {
            this.domainValidatorService = domainValidatorService;
            this.regiXService = regiXService;
        }

        public async Task<AgencyRegixDto> GetAgencyFromRegix(string uic)
        {
            var requestParams = new RegiXParameters()
            {
                CertificateThumbprint = AppSettingsProvider.AgencyRegixIntegration.CertificateThumbprint,
                OperationName = AppSettingsProvider.AgencyRegixIntegration.ActualAgencyOperationName,
                RequestBody = string.Format(@"<ActualStateRequestV2 
                        xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                        xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                        xmlns=""http://egov.bg/RegiX/AV/TR/ActualStateRequestV2"">
                        <UIC>{0}</UIC>
                        <FieldList>00030, 00050</FieldList>
                        </ActualStateRequestV2>", uic),
                EndpointConfiguration = EndpointConfiguration.BasicHttpBinding_IRegiXEntryPointV2,
                LawReason = AppSettingsProvider.AgencyRegixIntegration.LawReason,
                AdministrationName = AppSettingsProvider.AgencyRegixIntegration.AdministrationName,
                AdministrationOId = AppSettingsProvider.AgencyRegixIntegration.AdministrationOId
            };

            var response = new AgencyRegixDto();

            try
            {
                var result = await regiXService.GetData(requestParams);
                XmlSerializer serializer = new(typeof(AgencyRegixDto));

                using var reader = new XmlNodeReader(result.ServiceResultData.Data.Response.Any);
                response = (AgencyRegixDto)serializer.Deserialize(reader);

                return response;
            }
            catch (Exception)
            {
                domainValidatorService.ThrowErrorMessage(AgencyRegixErrorCode.AgencyRegix_UnableToConnectToAgencyRegix);
                return null;
            }
        }
    }
}
