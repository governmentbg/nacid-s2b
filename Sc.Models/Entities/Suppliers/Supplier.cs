using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Entities.Suppliers
{
    public class Supplier : EntityVersion
    {
        public SupplierType Type { get; set; }

        public int? InstitutionId { get; set; }
        [Skip]
        public Institution Institution { get; set; }

        public int? ComplexId { get; set; }
        [Skip]
        public Complex Complex { get; set; }

        [Skip]
        public SupplierRepresentative Representative { get; set; }
        [SkipUpdate]
        public List<SupplierOffering> SupplierOfferings { get; set; } = new List<SupplierOffering>();
    }
}
