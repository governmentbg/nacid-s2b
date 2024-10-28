using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup
{
    public class SupplierOfferingSearchDto : Entity
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string ShortDescription { get; set; }

        public string RepresentativeUserName { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeNameAlt { get; set; }
        public string RepresentativePhoneNumber { get; set; }

        public SupplierOfferingFileDto File { get; set; }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNameAlt { get; set; }

        // Institution
        public int? InstitutionId { get; set; }
        public int? RootInstitutionId { get; set; }
        public string RootInstitutionShortName { get; set; }
        public string RootInstitutionShortNameAlt { get; set; }
    }
}
