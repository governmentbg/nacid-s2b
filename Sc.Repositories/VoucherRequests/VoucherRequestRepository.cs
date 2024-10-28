using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Repositories.Base;

namespace Sc.Repositories.VoucherRequests
{
    public class VoucherRequestRepository : RepositoryBase<VoucherRequest, VoucherRequestFilterDto>, IVoucherRequestRepository
    {
        public VoucherRequestRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<VoucherRequest>, IQueryable<VoucherRequest>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.RequestCompany.Representative)
                    .Include(s => s.SupplierOffering.Supplier.Complex)
                    .Include(s => s.SupplierOffering.Supplier.Institution)
                    .Include(s => s.SupplierOffering.Supplier.Representative),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.RequestCompany.Representative)
                    .Include(s => s.SupplierOffering.Supplier.Complex)
                    .Include(s => s.SupplierOffering.Supplier.Institution)
                    .Include(s => s.SupplierOffering.Supplier.Representative),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
