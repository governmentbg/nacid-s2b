using Sc.Models.Dtos.ReceivedVouchers.Base;

namespace Sc.Models.Dtos.ReceivedVouchers
{
    public class ReceivedVoucherHistoryDto : BaseReceivedVoucherDto
    {
        public int ReceivedVoucherId { get; set; }
        public ReceivedVoucherHistoryFileDto File { get; set; }
    }
}
