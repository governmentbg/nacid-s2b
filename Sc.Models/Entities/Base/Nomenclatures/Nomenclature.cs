namespace Sc.Models.Entities.Base.Nomenclatures
{
    public abstract class Nomenclature : EntityVersion
    {
        public string Alias { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
