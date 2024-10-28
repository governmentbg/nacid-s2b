using Sc.Models.Enums.Common;

namespace Sc.Models.Dtos.Base.Nomenclatures
{
    public abstract class NomenclatureHierarchyDto<T> : NomenclatureDto
        where T : NomenclatureHierarchyDto<T>
    {
        public int? RootId { get; set; }
        public T Root { get; set; }
        public int? ParentId { get; set; }
        public T Parent { get; set; }
        public Level Level { get; set; }
    }
}
