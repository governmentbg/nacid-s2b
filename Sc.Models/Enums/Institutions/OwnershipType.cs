using System.ComponentModel;

namespace Sc.Models.Enums.Institutions
{
    [Description("Тип на собственост на институцията от РНД/РВУ")]
    public enum OwnershipType
    {
        [Description("Държавна")]
        State = 1,

        [Description("Частна")]
        Private = 2,

        [Description("Кооперативна")]
        Cooperative = 3,

        [Description("Смесено участие")]
        MixedParticipation = 4
    }
}
