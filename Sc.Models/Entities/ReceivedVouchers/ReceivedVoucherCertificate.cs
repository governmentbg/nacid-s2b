using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Suppliers;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherCertificate : EntityVersion
    {
        public int ReceivedVoucherId { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserFullname { get; set; }

        public ReceivedVoucherCertificateFile File { get; set; }

        public int SupplierId { get; set; }
        [Skip]
        public Supplier Supplier { get; set; }

        public int OfferingId { get; set; }
        [Skip]
        public SupplierOffering Offering { get; set; }
    }

    public class ReceivedVoucherCertificateConfiguration : IEntityTypeConfiguration<ReceivedVoucherCertificate>
    {
        public void Configure(EntityTypeBuilder<ReceivedVoucherCertificate> builder)
        {
            builder.Property(e => e.Username)
                .IsRequired();

            builder.Property(e => e.UserFullname)
                .IsRequired();
        }
    }
}
