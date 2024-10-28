using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.Suppliers
{
    public class SupplierOfferingFile : ScAttachedFile
    {
        public int SupplierOfferingId { get; set; }

        [Skip]
        public SupplierOffering Offering { get; set; }
    }


    public class SupplierOfferingFileConfiguration : IEntityTypeConfiguration<SupplierOfferingFile>
    {
        public void Configure(EntityTypeBuilder<SupplierOfferingFile> builder)
        {
            builder.HasOne(p => p.Offering)
               .WithMany(o => o.Files)
               .HasForeignKey(p => p.SupplierOfferingId);
        }
    }
}
