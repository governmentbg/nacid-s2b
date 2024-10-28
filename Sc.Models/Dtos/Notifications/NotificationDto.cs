using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Models.Enums.VoucherRequests;

namespace Sc.Models.Dtos.Notifications
{
    public class NotificationDto
    {
        public int EntityId { get; set; }

        public NotificationType Type { get; set; }
        public NotificationEntityType EntityType { get; set; }

        public int CompanyId { get; set; }
        public int OfferingId { get; set; }
        public int SupplierId { get; set; }

        public DateTime CreateDate { get; set; }

        public int FromUserId { get; set; }
        public string FromUsername { get; set; }
        public string FromFullname { get; set; }
        public string FromUserOrganization { get; set; }

        public int ToUserId { get; set; }

        public string Text { get; set; }

        // Only if EntityType == NotificationEntityType.VoucherRequest && Type == NotificationType.ChangedState
        public VoucherRequestState? VrState { get; set; }
        // Only if EntityType == NotificationEntityType.VoucherRequest && Type == NotificationType.GeneratedCode
        public string Code { get; set; }

        // Only if EntityType == NotificationEntityType.ReceivedVoucher && Type == NotificationType.ChangedState
        public ReceivedVoucherState? RvState { get; set; }
    }
}
