using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ReceivedVouchers
{
    public class ReceivedVoucherCertificateFilterDto : FilterDto<ReceivedVoucherCertificate>
    {
        public int? ReceivedVoucherId { get; set; }

        public override IQueryable<ReceivedVoucherCertificate> WhereBuilder(IQueryable<ReceivedVoucherCertificate> query)
        {
            if (ReceivedVoucherId.HasValue)
            {
                query = query.Where(e => e.ReceivedVoucherId == ReceivedVoucherId);
            }

            return query;
        }
    }
}
