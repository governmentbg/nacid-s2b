using System.ComponentModel;

namespace Sc.Models.Enums.ReceivedVouchers
{
    [Description("Статус на полученият ваучер")]
    public enum ReceivedVoucherState
    {
        [Description("Чернова")]
        Draft = 1,

        [Description("Въведени резултати")]
        Completed = 2,

        [Description("Генериран сертификат")]
        GeneratedCertificate = 3,

        [Description("Прекратен")]
        Terminated = 4
    }
}
