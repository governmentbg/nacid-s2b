using Sc.Models.Dtos.Base.Nomenclatures;

namespace Sc.Models.Dtos.Nomenclatures.SmartSpecializations
{
    public class SmartSpecializationDto : NomenclatureHierarchyDto<SmartSpecializationDto>
    {
        public string CodeNumber { get; set; }
    }
}
