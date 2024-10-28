using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Suppliers.Junctions;

namespace Sc.Models.Entities.Suppliers
{
    public class SupplierEquipment : EntityVersion
    {
        public int SupplierId { get; set; }
        [Skip]
        public Supplier Supplier { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SupplierEquipmentFile File { get; set; }

        public List<SupplierOfferingEquipment> SupplierOfferingEquipment { get; set; } = new List<SupplierOfferingEquipment>();
    }

    public class SupplierEquipmentConfiguration : IEntityTypeConfiguration<SupplierEquipment>
    {
        public void Configure(EntityTypeBuilder<SupplierEquipment> builder)
        {
            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);
        }
    }
}
