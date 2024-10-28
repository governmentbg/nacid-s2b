using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.VoucherRequests
{
    public class VoucherRequestCommunicationFilterDto : FilterDto<VoucherRequestCommunication>
    {
        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }

        public int? RequestCompanyId { get; set; }

        public int? SupplierId { get; set; }
        public int? SupplierOfferingId { get; set; }

        public override IQueryable<VoucherRequestCommunication> WhereBuilder(IQueryable<VoucherRequestCommunication> query)
        {
            if (FromCreateDate.HasValue)
            {
                query = query.Where(e => e.CreateDate.Date >= FromCreateDate.Value.Date);
            }

            if (ToCreateDate.HasValue)
            {
                query = query.Where(e => e.CreateDate.Date <= ToCreateDate.Value.Date);
            }

            if (RequestCompanyId.HasValue)
            {
                query = query.Where(e => e.Entity.RequestCompanyId == RequestCompanyId);
            }

            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.Entity.SupplierOffering.SupplierId == SupplierId);
            }

            if (SupplierOfferingId.HasValue)
            {
                query = query.Where(e => e.Entity.SupplierOfferingId == SupplierOfferingId);
            }

            return query;
        }
    }
}
