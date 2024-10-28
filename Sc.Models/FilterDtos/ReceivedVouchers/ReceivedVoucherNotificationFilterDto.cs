using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ReceivedVouchers
{
    public class ReceivedVoucherNotificationFilterDto : FilterDto<ReceivedVoucherNotification>
    {
        public int? ToUserId { get; set; }

        public int? CompanyId { get; set; }

        public int? SupplierId { get; set; }
        public int? OfferingId { get; set; }

        public override IQueryable<ReceivedVoucherNotification> WhereBuilder(IQueryable<ReceivedVoucherNotification> query)
        {
            if (ToUserId.HasValue)
            {
                query = query.Where(e => e.ToUserId == ToUserId);
            }

            if (CompanyId.HasValue)
            {
                query = query.Where(e => e.Entity.CompanyId == CompanyId);
            }

            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.Entity.SupplierId == SupplierId || e.Entity.SecondSupplierId == SupplierId);
            }

            if (OfferingId.HasValue)
            {
                query = query.Where(e => e.Entity.OfferingId == OfferingId || e.Entity.SecondOfferingId == OfferingId);
            }

            return query;
        }
    }
}
