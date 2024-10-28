using System.ComponentModel;

namespace Sc.Models.Enums.Complexes
{
    [Description("Тип инфраструктура")]
    public enum AreaOfActivity
    {
        [Description("Физически науки и инженерство")]
        ScienceAndEngineering = 1,

        [Description("Енергия")]
        Energy = 2,

        [Description("Околна среда")]
        Environment = 3,

        [Description("Здраве и храни")]
        Health = 4,

        [Description("Социални и хуманитарни науки")]
        Humanities = 5,

        [Description("Електронни инфраструктури")]
        Electronic = 6
    }
}
