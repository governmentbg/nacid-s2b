using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Models;
using Sc.Repositories.Base;
using Sc.Models.Enums.Common;
using Microsoft.EntityFrameworkCore;

namespace Sc.Repositories.Nomenclatures.Settlements
{
    public class MunicipalityRepository : RepositoryBase<Municipality, MunicipalityFilterDto>, IMunicipalityRepository
    {
        public MunicipalityRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<Municipality>, IQueryable<Municipality>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e.Include(s => s.District),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e.Include(s => s.District),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
