using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.Settlements
{
    public class DistrictRepository : RepositoryBase<District, DistrictFilterDto>, IDistrictRepository
    {
        public DistrictRepository(ScDbContext context)
            : base(context)
        {
        }
    }
}
