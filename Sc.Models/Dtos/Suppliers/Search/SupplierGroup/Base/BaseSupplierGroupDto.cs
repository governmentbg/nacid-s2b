using Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup;

namespace Sc.Models.Dtos.Suppliers.Search.SupplierGroup.Base
{
    public abstract class BaseSupplierGroupDto
    {
        public int? Id { get; set; }

        // Institution
        public string Uic { get; set; }
        public string Code { get; set; }

        // Common
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public int ViewOrder { get; set; }

        public List<SmartSpecializationGroupDto> SmartSpecializations { get; set; } = new List<SmartSpecializationGroupDto>();
    }
}
