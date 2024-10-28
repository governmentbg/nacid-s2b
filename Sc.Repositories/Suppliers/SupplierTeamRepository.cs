using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public class SupplierTeamRepository : RepositoryBase<SupplierTeam, SupplierTeamFilterDto>, ISupplierTeamRepository
    {
        public SupplierTeamRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<SupplierTeam>, IQueryable<SupplierTeam>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.SupplierOfferingTeams.OrderBy(m => m.SupplierOffering.ViewOrder).ThenBy(m => m.SupplierOffering.Name))
                        .ThenInclude(f => f.SupplierOffering)
                    .Include(s => s.Supplier.Institution.Root)
                    .Include(s => s.Supplier.Complex),
                IncludeType.Collections => e => e
                    .Include(s => s.SupplierOfferingTeams.OrderBy(m => m.SupplierOffering.ViewOrder).ThenBy(m => m.SupplierOffering.Name))
                        .ThenInclude(f => f.SupplierOffering),
                IncludeType.NavProperties => e => e
                    .Include(s => s.Supplier.Institution.Root)
                    .Include(s => s.Supplier.Complex),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
