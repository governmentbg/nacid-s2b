using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Suppliers.Junctions
{
    public class SupplierOfferingEquipmentDto : Entity
    {
        public int SupplierOfferingId { get; set; }
        public SupplierOfferingDto SupplierOffering { get; set; }

        public int SupplierEquipmentId { get; set; }
        public SupplierEquipmentDto SupplierEquipment { get; set; }
    }
}
