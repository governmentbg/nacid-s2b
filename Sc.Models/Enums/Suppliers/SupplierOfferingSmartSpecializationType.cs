using System.ComponentModel;

namespace Sc.Models.Enums.Suppliers
{
    [Description("Тип на тематичната област от ИСИС")]
    public enum SupplierOfferingSmartSpecializationType
    {
        [Description("Водеща")]
        Primary = 1,

        [Description("Допълнителна")]
        Secondary = 2
    }
}
