using System.ComponentModel;

namespace Sc.Models.Enums.Suppliers
{
    [Description("Тип на доставчика на услуга")]
    public enum SupplierType
    {
        [Description("Научна организация / Висше училище")]
        Institution = 1,

        [Description("Инфраструктура")]
        Complex = 2
    }
}
