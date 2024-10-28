using System.ComponentModel;

namespace Sc.Models.Enums.Notifications
{
    [Description("Тип на нотификацията")]
    public enum NotificationType
    {
        [Description("Съобщение")]
        Message = 1,

        [Description("Променен статус")]
        ChangedState = 2,

        [Description("Генериран код")]
        GeneratedCode = 3
    }
}
