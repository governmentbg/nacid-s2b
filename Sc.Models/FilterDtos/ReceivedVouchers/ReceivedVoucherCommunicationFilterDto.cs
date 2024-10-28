using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ReceivedVouchers
{
    public class ReceivedVoucherCommunicationFilterDto : FilterDto<ReceivedVoucherCommunication>
    {
        public int? ReceivedVoucherId { get; set; }

        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }

        public int? CompanyId { get; set; }

        public int? SupplierId { get; set; }
        public int? OfferingId { get; set; }

        public override IQueryable<ReceivedVoucherCommunication> WhereBuilder(IQueryable<ReceivedVoucherCommunication> query)
        {
            if (ReceivedVoucherId.HasValue)
            {
                query = query.Where(e => e.EntityId == ReceivedVoucherId);
            }

            if (FromCreateDate.HasValue)
            {
                query = query.Where(e => e.CreateDate.Date >= FromCreateDate.Value.Date);
            }

            if (ToCreateDate.HasValue)
            {
                query = query.Where(e => e.CreateDate.Date <= ToCreateDate.Value.Date);
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
