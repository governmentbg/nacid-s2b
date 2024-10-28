using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.FilterDtos.Nomenclatures.Institutions;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.Institutions
{
    public interface IInstitutionRepository : IRepositoryBase<Institution, InstitutionFilterDto>
    {
    }
}
