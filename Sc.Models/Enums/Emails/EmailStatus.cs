using System.ComponentModel;

namespace Sc.Models.Enums.Emails
{
    [Description("Статус на имейла")]
    public enum EmailStatus
    {
        [Description("Чакащ за изпращане")]
        Unsent = 1,

        [Description("В процес на изпращане")]
        Pending = 2,

        [Description("Изпратен")]
        Delivered = 3,

        [Description("Неуспешно изпращане")]
        Failed = 4
    }
}
