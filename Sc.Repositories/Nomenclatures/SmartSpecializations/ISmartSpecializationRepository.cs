using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Models.FilterDtos.Nomenclatures.SmartSpecializations;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.SmartSpecializations
{
    public interface ISmartSpecializationRepository : IRepositoryBase<SmartSpecialization, SmartSpecializationFilterDto>
    {
    }
}
