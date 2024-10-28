using AutoMapper;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Base;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Companies;
using Sc.Models.Entities.Companies;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Companies;
using Sc.Repositories.Companies;
using Sc.Services.Base;

namespace Sc.Services.Companies
{
    public class CompanyService : ValidateService<Company, CompanyDto>
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;

        public CompanyService(
            IMapper mapper,
            ICompanyRepository companyRepository,
            DomainValidatorService domainValidatorService
            )
            : base(domainValidatorService)
        {
            this.mapper = mapper;
            this.companyRepository = companyRepository;
        }

        public async Task<CompanyDto> GetById(int id, CancellationToken cancellationToken)
        {
            var company = await companyRepository.GetById(id, cancellationToken, companyRepository.ConstructInclude(IncludeType.NavProperties));

            return mapper.Map<CompanyDto>(company);
        }

        public async Task<CompanyDto> GetByUic(string uic, CancellationToken cancellationToken)
        {
            var company = await companyRepository.GetByProperties(e => e.Uic == uic.Trim(), cancellationToken, companyRepository.ConstructInclude(IncludeType.None));

            return mapper.Map<CompanyDto>(company);
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(CompanyFilterDto filterDto, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await companyRepository.GetAll(filterDto, cancellationToken, companyRepository.ConstructInclude(IncludeType.NavProperties), e => e.OrderBy(e => e.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<CompanyDto> Create(CompanyDto companyDto, CancellationToken cancellationToken)
        {
            companyDto.ValidateProperties(domainValidatorService);
            await CreateValidation(companyDto, cancellationToken);

            var newCompany = mapper.Map<Company>(companyDto);
            newCompany.IsActive = true;

            await companyRepository.Create(newCompany);

            return mapper.Map<CompanyDto>(await companyRepository.GetById(newCompany.Id, cancellationToken));
        }

        public async Task<bool> ChangeIsActive(Company companyForUpdate, IsActiveDto isActiveDto)
        {
            await companyRepository.UpdateFromDto(companyForUpdate, isActiveDto);

            return companyForUpdate.IsActive;
        }

        #region IValidationService
        protected async override Task CreateValidation(CompanyDto dto, CancellationToken cancellationToken)
        {
            if (await companyRepository.AnyEntity(e => e.Uic == dto.Uic, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Entity_Exists);
            }
        }
        #endregion
    }
}
