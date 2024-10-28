using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Base;

namespace Sc.Repositories.Suppliers
{
    public class SupplierRepository : RepositoryBase<Supplier, SupplierFilterDto>, ISupplierRepository
    {
        public SupplierRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<Supplier>, IQueryable<Supplier>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Representative)
                    .Include(s => s.Institution.Root)
                    .Include(s => s.Institution.Parent)
                    .Include(s => s.Institution.District)
                    .Include(s => s.Institution.Municipality)
                    .Include(s => s.Institution.Settlement)
                    .Include(s => s.Institution.Children)
                    .Include(s => s.Complex.District)
                    .Include(s => s.Complex.Municipality)
                    .Include(s => s.Complex.Settlement)
                    .Include(s => s.SupplierOfferings)
                        .ThenInclude(f => f.Files),
                IncludeType.Collections => e => e
                    .Include(s => s.SupplierOfferings),
                IncludeType.NavProperties => e => e
                    .Include(s => s.Representative)
                    .Include(s => s.Institution.Root)
                    .Include(s => s.Institution.Parent)
                    .Include(s => s.Institution.District)
                    .Include(s => s.Institution.Municipality)
                    .Include(s => s.Institution.Settlement)
                    .Include(s => s.Institution.Children)
                    .Include(s => s.Complex.District)
                    .Include(s => s.Complex.Municipality)
                    .Include(s => s.Complex.Settlement),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
