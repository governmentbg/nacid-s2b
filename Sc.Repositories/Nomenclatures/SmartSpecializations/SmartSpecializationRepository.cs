using Sc.Models;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Models.FilterDtos.Nomenclatures.SmartSpecializations;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.SmartSpecializations
{
    public class SmartSpecializationRepository : RepositoryBase<SmartSpecialization, SmartSpecializationFilterDto>, ISmartSpecializationRepository
    {
        public SmartSpecializationRepository(ScDbContext context)
            : base(context)
        {
        }
    }
}
