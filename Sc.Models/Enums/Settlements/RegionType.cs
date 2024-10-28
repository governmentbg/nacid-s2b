using System.ComponentModel;

namespace Sc.Models.Enums.Settlements
{
    [Description("Регион")]
    public enum RegionType
    {
        [Description("Югозападен")]
        SouthWest = 999999991,

        [Description("Югоизточен")]
        SouthEast = 999999992,

        [Description("Североизточен")]
        NorthEast = 999999993,

        [Description("Северен централен")]
        NorthMiddle = 999999994,

        [Description("Северозападен")]
        NorthWest = 999999995,

        [Description("Южен централен")]
        SouthMiddle = 999999996
    }
}
