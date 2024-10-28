using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Models;
using Sc.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Sc.Repositories.VoucherRequests
{
    public class VoucherRequestNotificationRepository : RepositoryBase<VoucherRequestNotification, VoucherRequestNotificationFilterDto>, IVoucherRequestNotificationRepository
    {
        public VoucherRequestNotificationRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<VoucherRequestNotification>, IQueryable<VoucherRequestNotification>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Entity.SupplierOffering.Supplier.Complex)
                    .Include(s => s.Entity.SupplierOffering.Supplier.Institution),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.Entity),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
