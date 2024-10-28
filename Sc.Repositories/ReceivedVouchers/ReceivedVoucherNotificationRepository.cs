using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.Base;

namespace Sc.Repositories.ReceivedVouchers
{
    public class ReceivedVoucherNotificationRepository : RepositoryBase<ReceivedVoucherNotification, ReceivedVoucherNotificationFilterDto>, IReceivedVoucherNotificationRepository
    {
        public ReceivedVoucherNotificationRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<ReceivedVoucherNotification>, IQueryable<ReceivedVoucherNotification>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Entity.Supplier.Complex)
                    .Include(s => s.Entity.Supplier.Institution)
                    .Include(s => s.Entity.SecondSupplier.Complex)
                    .Include(s => s.Entity.SecondSupplier.Institution),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.Entity),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
