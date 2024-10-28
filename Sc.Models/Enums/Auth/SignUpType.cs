using System.ComponentModel;

namespace Sc.Models.Enums.Auth
{
    [Description("Тип на регистрацията")]
    public enum SignUpType
    {
        [Description("Доставчик на услуга")]
        Supplier = 1,

        [Description("Предприятие")]
        Company = 2
    }
}
