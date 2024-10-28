using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;

namespace Sc.Models.Entities.Suppliers
{
    public class SupplierRepresentative : EntityVersion
    {
        [Skip]
        public Supplier Supplier { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class SupplierRepresentativeConfiguration : IEntityTypeConfiguration<SupplierRepresentative>
    {
        public void Configure(EntityTypeBuilder<SupplierRepresentative> builder)
        {
            builder.HasOne(p => p.Supplier)
                   .WithOne(a => a.Representative)
                   .HasForeignKey<SupplierRepresentative>(p => p.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.NameAlt)
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(18);
        }
    }
}
