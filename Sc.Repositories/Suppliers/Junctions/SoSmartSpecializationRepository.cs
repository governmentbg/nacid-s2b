using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Suppliers.Junctions;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers.Junctions;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers.Junctions
{
    public class SoSmartSpecializationRepository : RepositoryBase<SupplierOfferingSmartSpecialization, SupplierOfferingSmartSpecializationFilterDto>, ISoSmartSpecializationRepository
    {
        public SoSmartSpecializationRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<SupplierOfferingSmartSpecialization>, IQueryable<SupplierOfferingSmartSpecialization>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(e => e.SmartSpecialization.Root)
                    .Include(e => e.SupplierOffering.Supplier.Institution.Root)
                    .Include(e => e.SupplierOffering.Supplier.Complex),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(e => e.SmartSpecialization)
                    .Include(e => e.SupplierOffering),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
