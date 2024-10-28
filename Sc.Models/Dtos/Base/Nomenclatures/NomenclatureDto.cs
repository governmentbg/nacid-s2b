using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Base.Nomenclatures
{
    public abstract class NomenclatureDto : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public bool IsActive { get; set; }
    }
}
