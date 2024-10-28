using System.ComponentModel;

namespace Sc.Models.Enums.Complexes
{
    public enum OrganizationTypeEnum
    {
        [Description("Финансираща организация")]
        Financing = 1,

        [Description("Научна организация")]
        Scientific = 2,

        [Description("Извън РНД")]
        OutOfRND = 3
    }
}
