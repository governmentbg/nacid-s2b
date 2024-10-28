using System.ComponentModel;

namespace Sc.Models.Enums.Common
{
    [Description("Тип на търсенето")]
    public enum KeywordsSearchType
    {
        [Description("Точно съвпадение")]
        ExactMatch = 1,

        [Description("Всички думи")]
        MatchAll = 2,

        [Description("Поне една дума")]
        MatchAny = 3
    }
}
