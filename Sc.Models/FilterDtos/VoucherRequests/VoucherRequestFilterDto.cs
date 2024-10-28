using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.VoucherRequests;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.VoucherRequests
{
    public class VoucherRequestFilterDto : FilterDto<VoucherRequest>
    {
        public bool? HasGeneratedCode { get; set; }

        public VoucherRequestState? State { get; set; }

        public DateTime? FromCreateDate { get; set; }
        public DateTime? ToCreateDate { get; set; }

        public int? RequestCompanyId { get; set; }

        public int? SupplierId { get; set; }
        public int? SupplierOfferingId { get; set; }

        // Server only
        // This is not null only if user is Institution or Complex authorized representatitve
        public int? AuthorizedRepresentativeUserId { get; set; }

        public override IQueryable<VoucherRequest> WhereBuilder(IQueryable<VoucherRequest> query)
        {
            if (HasGeneratedCode.HasValue)
            {
                if (HasGeneratedCode.Value)
                {
                    query = query.Where(e => !string.IsNullOrWhiteSpace(e.Code));
                }
                else
                {
                    query = query.Where(e => string.IsNullOrWhiteSpace(e.Code));
                }
            }

            if (State.HasValue)
            {
                query = query.Where(e => e.State == State);
            }

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
                query = query.Where(e => e.RequestCompanyId == RequestCompanyId);
            }

            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.SupplierOffering.SupplierId == SupplierId);
            }

            if (SupplierOfferingId.HasValue)
            {
                query = query.Where(e => e.SupplierOfferingId == SupplierOfferingId);
            }

            if (AuthorizedRepresentativeUserId.HasValue)
            {
                query = query.Where(e => e.SupplierOffering.Supplier.Representative.UserId == AuthorizedRepresentativeUserId.Value
                    || e.SupplierOffering.SupplierOfferingTeams.Any(s => s.SupplierTeam.UserId == AuthorizedRepresentativeUserId.Value));
            }

            return query;
        }
    }
}
