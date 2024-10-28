using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.VoucherRequests
{
    public class VoucherRequestNotificationFilterDto : FilterDto<VoucherRequestNotification>
    {
        public int? ToUserId { get; set; }

        public int? CompanyId { get; set; }

        public int? SupplierId { get; set; }
        public int? OfferingId { get; set; }

        public override IQueryable<VoucherRequestNotification> WhereBuilder(IQueryable<VoucherRequestNotification> query)
        {
            if (ToUserId.HasValue)
            {
                query = query.Where(e => e.ToUserId == ToUserId);
            }

            if (CompanyId.HasValue)
            {
                query = query.Where(e => e.Entity.RequestCompanyId == CompanyId);
            }

            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.Entity.SupplierOffering.SupplierId == SupplierId);
            }

            if (OfferingId.HasValue)
            {
                query = query.Where(e => e.Entity.SupplierOfferingId == OfferingId);
            }

            return query;
        }
    }
}
