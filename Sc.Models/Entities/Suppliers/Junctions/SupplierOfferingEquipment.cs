using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;

namespace Sc.Models.Entities.Suppliers.Junctions
{
    public class SupplierOfferingEquipment : EntityVersion
    {
        public int SupplierOfferingId { get; set; }
        [Skip]
        public SupplierOffering SupplierOffering { get; set; }

        public int SupplierEquipmentId { get; set; }
        [Skip]
        public SupplierEquipment SupplierEquipment { get; set; }
    }

    public class SupplierOfferingEquipmentConfiguration : IEntityTypeConfiguration<SupplierOfferingEquipment>
    {
        public void Configure(EntityTypeBuilder<SupplierOfferingEquipment> builder)
        {
            builder.HasIndex(e => new { e.SupplierOfferingId, e.SupplierEquipmentId })
                .IsUnique();
        }
    }
}
