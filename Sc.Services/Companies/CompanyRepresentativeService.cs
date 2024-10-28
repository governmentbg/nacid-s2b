using AutoMapper;
using Sc.Models.Dtos.Companies;
using Sc.Models.Entities.Companies;
using Sc.Repositories.Companies;

namespace Sc.Services.Companies
{
    public class CompanyRepresentativeService
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepresentativeRepository companyRepresentativeRepository;

        public CompanyRepresentativeService(
            IMapper mapper,
            ICompanyRepresentativeRepository companyRepresentativeRepository)
        {
            this.mapper = mapper;
            this.companyRepresentativeRepository = companyRepresentativeRepository;
        }

        public async Task<CompanyRepresentativeDto> Create(int companyId, CompanyRepresentativeDto companyRepresentativeDto, CancellationToken cancellationToken)
        {
            companyRepresentativeDto.Id = companyId;

            var newCompanyRepresentative = mapper.Map<CompanyRepresentative>(companyRepresentativeDto);
            await companyRepresentativeRepository.Create(newCompanyRepresentative);

            return mapper.Map<CompanyRepresentativeDto>(await companyRepresentativeRepository.GetById(newCompanyRepresentative.Id, cancellationToken));
        }

        public async Task<bool> Exists(int companyId, CancellationToken cancellationToken)
        {
            return await companyRepresentativeRepository.AnyEntity(e => e.Id == companyId, cancellationToken);
        }
    }
}
