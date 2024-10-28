using Sc.Models.Entities.Companies;
using Sc.Models.FilterDtos.Companies;
using Sc.Repositories.Base;

namespace Sc.Repositories.Companies
{
    public interface ICompanyRepresentativeRepository : IRepositoryBase<CompanyRepresentative, CompanyRepresentativeFilterDto>
    {
    }
}
