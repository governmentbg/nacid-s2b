using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.Settlements
{
    public interface IMunicipalityRepository : IRepositoryBase<Municipality, MunicipalityFilterDto>
    {
    }
}
