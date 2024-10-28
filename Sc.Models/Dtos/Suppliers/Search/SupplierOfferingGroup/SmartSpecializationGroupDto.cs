using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup
{
    public class SmartSpecializationGroupDto : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public int ViewOrder { get; set; }

        public int OfferingsCount { get { return Offerings.Count; } }
        public List<SmartSpecializationOfferingGroupDto> Offerings { get; set; } = new List<SmartSpecializationOfferingGroupDto>();
    }
}
