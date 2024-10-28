using Sc.Models.Entities.Base;
using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup
{
    public class SmartSpecializationOfferingGroupDto : Entity
    {
        public string Name { get; set; }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNameAlt { get; set; }

        // Institution
        public int? InstitutionId { get; set; }
        public int? RootInstitutionId { get; set; }
        public string RootInstitutionShortName { get; set; }
        public string RootInstitutionShortNameAlt { get; set; }

        public string ShortDescription { get; set; }
        public OfferingType OfferingType { get; set; }
    }
}
