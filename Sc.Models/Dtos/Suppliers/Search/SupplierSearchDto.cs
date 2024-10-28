using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Suppliers.Search
{
    public class SupplierSearchDto : Entity
    {
        // Common
        public string Name { get; set; }
        public string NameAlt { get; set; }

        // Institution
        public string Uic { get; set; }
        public int? RootId { get; set; }
        public string RootName { get; set; }
        public string RootNameAlt { get; set; }

        public List<SupplierOfferingDto> SupplierOfferings { get; set; } = new List<SupplierOfferingDto>();
    }
}
