using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.ReceivedVouchers
{
    public class ReceivedVoucherHistoryFilterDto : FilterDto<ReceivedVoucherHistory>
    {
        public int ReceivedVoucherId { get; set; }

        public override IQueryable<ReceivedVoucherHistory> WhereBuilder(IQueryable<ReceivedVoucherHistory> query)
        {
            query = query.Where(e => e.ReceivedVoucherId == ReceivedVoucherId);

            return query;
        }
    }
}
