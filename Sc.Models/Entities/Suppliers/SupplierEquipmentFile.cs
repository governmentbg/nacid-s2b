using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.Suppliers
{
    public class SupplierEquipmentFile : ScAttachedFile
    {
        [Skip]
        public SupplierEquipment Equipment { get; set; }
    }

    public class SupplierEquipmentFileConfiguration : IEntityTypeConfiguration<SupplierEquipmentFile>
    {
        public void Configure(EntityTypeBuilder<SupplierEquipmentFile> builder)
        {
            builder.HasOne(p => p.Equipment)
                   .WithOne(a => a.File)
                   .HasForeignKey<SupplierEquipmentFile>(p => p.Id);
        }
    }
}
