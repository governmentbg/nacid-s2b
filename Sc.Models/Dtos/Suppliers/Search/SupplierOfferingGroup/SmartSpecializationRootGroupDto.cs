using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup
{
    public class SmartSpecializationRootGroupDto : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public int ViewOrder { get; set; }

        public List<SmartSpecializationGroupDto> SmartSpecializations { get; set; } = new List<SmartSpecializationGroupDto>();
    }
}
