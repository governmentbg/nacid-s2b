using Sc.Models.Entities.Notifications;
using Sc.Models.Enums.VoucherRequests;

namespace Sc.Models.Entities.VoucherRequests
{
    public class VoucherRequestNotification : BaseNotification<VoucherRequest>
    {
        // Only if Type == NotificationType.ChangedState
        public VoucherRequestState? ChangedToState { get; set; }

        // Only if Type == NotificationType.GeneratedCode
        public string Code { get; set; }
    }
}
