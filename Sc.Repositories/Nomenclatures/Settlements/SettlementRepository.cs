using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Models;
using Sc.Repositories.Base;
using Sc.Models.Enums.Common;
using Microsoft.EntityFrameworkCore;

namespace Sc.Repositories.Nomenclatures.Settlements
{
    public class SettlementRepository : RepositoryBase<Settlement, SettlementFilterDto>, ISettlementRepository
    {
        public SettlementRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<Settlement>, IQueryable<Settlement>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.District)
                    .Include(s => s.Municipality),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.District)
                    .Include(s => s.Municipality),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
