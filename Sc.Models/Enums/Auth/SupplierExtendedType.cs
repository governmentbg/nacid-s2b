using System.ComponentModel;

namespace Sc.Models.Enums.Auth
{
    [Description("Тип на доставчика на услуга - разширено")]
    public enum SupplierExtendedType
    {
        [Description("Висше училище")]
        University = 1,

        [Description("Научна организация")]
        ScientificOrganization = 2,

        [Description("Научна инфраструктура")]
        Complex = 3
    }
}
