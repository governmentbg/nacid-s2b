using AutoMapper;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Companies;
using Sc.Models.Entities.Companies;
using Sc.Models.Enums.Common;
using Sc.Repositories.Companies;
using Sc.Services.Base;

namespace Sc.Services.Companies
{
    public class CompanyAdditionalService : ValidateService<CompanyAdditional, CompanyAdditionalDto>
    {
        private readonly IMapper mapper;
        private readonly ICompanyAdditionalRepository companyAdditionalRepository;

        public CompanyAdditionalService(
            IMapper mapper,
            ICompanyAdditionalRepository companyAdditionalRepository,
            DomainValidatorService domainValidatorService
            )
            : base(domainValidatorService)
        {
            this.mapper = mapper;
            this.companyAdditionalRepository = companyAdditionalRepository;
        }

        public async Task<CompanyAdditionalDto> GetDtoById(int id, CancellationToken cancellationToken)
        {
            var company = await companyAdditionalRepository.GetById(id, cancellationToken, companyAdditionalRepository.ConstructInclude(IncludeType.None));

            return mapper.Map<CompanyAdditionalDto>(company);
        }

        public async Task<CompanyAdditionalDto> Create(int id, CompanyAdditionalDto companyAdditionalDto, CancellationToken cancellationToken)
        {
            companyAdditionalDto.Id = id;
            companyAdditionalDto.ValidateProperties(domainValidatorService);
            await CreateValidation(companyAdditionalDto, cancellationToken);

            var newCompanyAdditional = mapper.Map<CompanyAdditional>(companyAdditionalDto);

            await companyAdditionalRepository.Create(newCompanyAdditional);

            return mapper.Map<CompanyAdditionalDto>(await companyAdditionalRepository.GetById(newCompanyAdditional.Id, cancellationToken));
        }

        public async Task<CompanyAdditionalDto> Update(CompanyAdditional companyAdditionalForUpdate, CompanyAdditionalDto companyAdditionalDto, CancellationToken cancellationToken)
        {
            companyAdditionalDto.ValidateProperties(domainValidatorService);

            await companyAdditionalRepository.UpdateFromDto(companyAdditionalForUpdate, companyAdditionalDto);

            return await GetDtoById(companyAdditionalForUpdate.Id, cancellationToken);
        }

        #region IValidationService
        protected async override Task CreateValidation(CompanyAdditionalDto dto, CancellationToken cancellationToken)
        {
            if (await companyAdditionalRepository.AnyEntity(e => e.Id == dto.Id, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.CompanyAdditional_Entity_Exists);
            }
        }
        #endregion
    }
}
