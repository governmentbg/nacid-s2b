using System.ComponentModel;

namespace Sc.Models.Enums.Notifications
{
    [Description("Обект на нотификацията")]
    public enum NotificationEntityType
    {
        [Description("Заявка за код")]
        VoucherRequest = 1,

        [Description("Получен ваучер")]
        ReceivedVoucher = 2
    }
}
