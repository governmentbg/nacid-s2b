using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.ReceivedVouchers.Base;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucher : BaseReceivedVoucher
    {
        public ReceivedVoucherFile File { get; set; }

        [Skip]
        public List<ReceivedVoucherCertificate> Certificates { get; set; } = new List<ReceivedVoucherCertificate>();
        [Skip]
        public List<ReceivedVoucherHistory> Histories { get; set; } = new List<ReceivedVoucherHistory>();
    }

    public class ReceivedVoucherConfiguration : IEntityTypeConfiguration<ReceivedVoucher>
    {
        public void Configure(EntityTypeBuilder<ReceivedVoucher> builder)
        {
            builder.HasMany(e => e.Certificates)
                .WithOne()
                .HasForeignKey(e => e.ReceivedVoucherId);

            builder.HasMany(e => e.Histories)
                .WithOne()
                .HasForeignKey(e => e.ReceivedVoucherId);

            builder.HasIndex(e => e.ContractNumber)
                .IsUnique();

            builder.Property(e => e.ContractNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.ContractDate)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.OfferingClarifications)
                .HasMaxLength(500);

            builder.Property(e => e.SecondOfferingClarifications)
                .HasMaxLength(500);
        }
    }
}
