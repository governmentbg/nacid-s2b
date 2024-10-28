using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public class SupplierOfferingRepository : RepositoryBase<SupplierOffering, SupplierOfferingFilterDto>, ISupplierOfferingRepository
    {
        public SupplierOfferingRepository(ScDbContext context)
            : base(context)
        {
        }

        public async Task<SupplierOffering> GetByIdAndSupplierId(int id, int supplierId, CancellationToken cancellationToken, Func<IQueryable<SupplierOffering>, IQueryable<SupplierOffering>> includesFunc = null)
        {
            var query = context.Set<SupplierOffering>().AsNoTracking();

            if (includesFunc != null)
            {
                query = includesFunc(query);
            }

            return await query.SingleOrDefaultAsync(e => e.Id == id && e.SupplierId == supplierId, cancellationToken);
        }

        public override Func<IQueryable<SupplierOffering>, IQueryable<SupplierOffering>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Supplier.Institution.Root)
                    .Include(s => s.Supplier.Complex)
                    .Include(e => e.District)
                    .Include(e => e.Municipality)
                    .Include(e => e.Settlement)
                    .Include(s => s.Files)
                    .Include(s => s.SmartSpecializations.OrderBy(e => e.Type).ThenBy(e => e.SmartSpecialization.ViewOrder).ThenBy(e => e.SmartSpecialization.Name))
                        .ThenInclude(m => m.SmartSpecialization.Root)
                    .Include(s => s.SupplierOfferingTeams.OrderBy(e => e.SupplierTeam.ViewOrder).ThenBy(e => e.SupplierTeam.Name))
                        .ThenInclude(s => s.SupplierTeam)
                    .Include(s => s.SupplierOfferingEquipment.OrderBy(e => e.SupplierEquipment.ViewOrder).ThenBy(e => e.SupplierEquipment.Name))
                        .ThenInclude(s => s.SupplierEquipment),
                IncludeType.Collections => e => e
                    .Include(e => e.District)
                    .Include(e => e.Municipality)
                    .Include(e => e.Settlement)
                    .Include(s => s.Files)
                    .Include(s => s.SmartSpecializations.OrderBy(e => e.Type).ThenBy(e => e.SmartSpecialization.ViewOrder).ThenBy(e => e.SmartSpecialization.Name))
                        .ThenInclude(m => m.SmartSpecialization)
                    .Include(s => s.SupplierOfferingTeams.OrderBy(e => e.SupplierTeam.ViewOrder).ThenBy(e => e.SupplierTeam.Name))
                        .ThenInclude(s => s.SupplierTeam)
                    .Include(s => s.SupplierOfferingEquipment.OrderBy(e => e.SupplierEquipment.ViewOrder).ThenBy(e => e.SupplierEquipment.Name))
                        .ThenInclude(s => s.SupplierEquipment),
                IncludeType.NavProperties => e => e
                    .Include(s => s.Supplier.Institution)
                    .Include(s => s.Supplier.Complex)
                    .Include(s => s.Files)
                    .Include(e => e.District)
                    .Include(e => e.Municipality)
                    .Include(e => e.Settlement),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
