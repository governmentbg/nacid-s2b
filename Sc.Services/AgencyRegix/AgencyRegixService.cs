using AutoMapper;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes.Integrations;
using Integrations.AgencyRegixIntegration;
using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Nomenclatures;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Enums.Common;
using Sc.Repositories.Nomenclatures;
using Sc.Repositories.Nomenclatures.Settlements;

namespace Sc.Services.AgencyRegix
{
    public class AgencyRegixService
    {
        private readonly AgencyRegixIntegrationService agencyRegixIntegrationService;
        private readonly ILawFormRepository lawFormRepository;
        private readonly ISettlementRepository settlementRepository;
        private readonly IMapper mapper;
        private readonly DomainValidatorService domainValidatorService;

        public AgencyRegixService(
            AgencyRegixIntegrationService agencyRegixIntegrationService,
            ILawFormRepository lawFormRepository,
            ISettlementRepository settlementRepository,
            IMapper mapper,
            DomainValidatorService domainValidatorService
            )
        {
            this.agencyRegixIntegrationService = agencyRegixIntegrationService;
            this.lawFormRepository = lawFormRepository;
            this.settlementRepository = settlementRepository;
            this.mapper = mapper;
            this.domainValidatorService = domainValidatorService;
        }

        public async Task<CompanyDto> GetCompanyFromAgencyRegix(string uic, CancellationToken cancellationToken)
        {
            var agencyRegixDto = await agencyRegixIntegrationService.GetAgencyFromRegix(uic);

            if (agencyRegixDto.Deed == null || agencyRegixDto.Deed.Records.Count < 2)
            {
                domainValidatorService.ThrowErrorMessage(AgencyRegixErrorCode.AgencyRegix_Company_NotFound);
            }

            var regixLawForm = agencyRegixDto.Deed.Records.First();
            var regixSettlement = agencyRegixDto.Deed.Records.Skip(1).First();

            var lawForm = await lawFormRepository.GetByProperties(e => e.Name.Trim().ToLower() == regixLawForm.RecordData.LegalForm.Text.Trim().ToLower(), cancellationToken, lawFormRepository.ConstructInclude(IncludeType.None));

            if (lawForm == null)
            {
                domainValidatorService.ThrowErrorMessage(AgencyRegixErrorCode.AgencyRegix_LawForm_Invalid);
            }

            var settlement = await settlementRepository.GetByProperties(e => e.Code.Trim().ToLower() == regixSettlement.RecordData.Seat.Address.SettlementEKATTE.Trim().ToLower(), cancellationToken, settlementRepository.ConstructInclude(IncludeType.NavProperties));

            var companyDto = new CompanyDto
            {
                Uic = uic,
                Name = agencyRegixDto.Deed.CompanyName,
                LawForm = mapper.Map<LawFormDto>(lawForm),
                LawFormId = lawForm.Id,
                Settlement = settlement != null ? mapper.Map<SettlementDto>(settlement) : null,
                SettlementId = settlement != null ? settlement.Id : 0,
                Municipality = settlement != null ? mapper.Map<MunicipalityDto>(settlement.Municipality) : null,
                MunicipalityId = settlement != null ? settlement.MunicipalityId : 0,
                District = settlement != null ? mapper.Map<DistrictDto>(settlement.District) : null,
                DistrictId = settlement != null ? settlement.DistrictId : 0,
                Address = $"{regixSettlement.RecordData.Seat.Address.Area} {regixSettlement.RecordData.Seat.Address.Street} {regixSettlement.RecordData.Seat.Address.StreetNumber} {regixSettlement.RecordData.Seat.Address.Block} {regixSettlement.RecordData.Seat.Address.Entrance} {regixSettlement.RecordData.Seat.Address.Floor} {regixSettlement.RecordData.Seat.Address.Apartment}".Trim(),
                IsActive = true            
            };

            return companyDto;
        }
    }
}
