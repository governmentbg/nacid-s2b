using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public class SupplierRepresentativeRepository : RepositoryBase<SupplierRepresentative, SupplierRepresentativeFilterDto>, ISupplierRepresentativeRepository
    {
        public SupplierRepresentativeRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<SupplierRepresentative>, IQueryable<SupplierRepresentative>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Supplier.Institution)
                    .Include(s => s.Supplier.Complex),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.Supplier.Institution)
                    .Include(s => s.Supplier.Complex),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
