using Sc.Models.Enums.Common;

namespace Sc.Models.Entities.Base.Nomenclatures
{
    public abstract class NomenclatureHierarchy : Nomenclature
    {
        public int? RootId { get; set; }
        public int? ParentId { get; set; }
        public Level Level { get; set; }
    }
}
