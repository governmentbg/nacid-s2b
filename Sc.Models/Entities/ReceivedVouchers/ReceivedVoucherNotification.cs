using Sc.Models.Entities.Notifications;
using Sc.Models.Enums.ReceivedVouchers;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherNotification : BaseNotification<ReceivedVoucher>
    {
        // Only if Type == NotificationType.ChangedState
        public ReceivedVoucherState? ChangedToState { get; set; }
    }
}
