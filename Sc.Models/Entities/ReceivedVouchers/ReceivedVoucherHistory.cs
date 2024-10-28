using Sc.Models.Entities.ReceivedVouchers.Base;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherHistory : BaseReceivedVoucher
    {
        public int ReceivedVoucherId { get; set; }
        public ReceivedVoucherHistoryFile File { get; set; }
    }
}
