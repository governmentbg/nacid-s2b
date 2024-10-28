using Sc.Models.Dtos.Nomenclatures.SmartSpecializations;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Dtos.Suppliers.Junctions
{
    public class SupplierOfferingSmartSpecializationDto : Entity
    {
        public int SupplierOfferingId { get; set; }

        public int SmartSpecializationId { get; set; }
        public SmartSpecializationDto SmartSpecialization { get; set; }

        public SupplierOfferingSmartSpecializationType Type { get; set; }
    }
}
