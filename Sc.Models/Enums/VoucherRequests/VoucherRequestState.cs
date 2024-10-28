using System.ComponentModel;

namespace Sc.Models.Enums.VoucherRequests
{
    [Description("Статус на заявката за ваучер")]
    public enum VoucherRequestState
    {
        [Description("Чернова")]
        Draft = 1,

        [Description("В очакване на заявка за код")]
        Pending = 2,

        [Description("Заявка за код")]
        Requested = 3,

        [Description("Генериран код")]
        Generated = 4,

        [Description("Получен ваучер")]
        Recieved = 5,

        [Description("Отказан")]
        Declined = 6
    }
}
