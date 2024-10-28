using System.ComponentModel;

namespace Sc.Models.Enums.Companies
{
    [Description("Вид предприятие")]
    public enum CompanyType
    {
        [Description("Микро")]
        Micro = 1,

        [Description("Малко")]
        Small = 2,

        [Description("Средно")]
        Medium = 3
    }
}
