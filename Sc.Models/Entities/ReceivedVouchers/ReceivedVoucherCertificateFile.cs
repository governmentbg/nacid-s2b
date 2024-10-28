using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherCertificateFile : ScAttachedFile
    {
        [Skip]
        public ReceivedVoucherCertificate Certificate { get; set; }
    }

    public class ReceivedVoucherCertificateFileConfiguration : IEntityTypeConfiguration<ReceivedVoucherCertificateFile>
    {
        public void Configure(EntityTypeBuilder<ReceivedVoucherCertificateFile> builder)
        {
            builder.HasOne(p => p.Certificate)
                   .WithOne(a => a.File)
                   .HasForeignKey<ReceivedVoucherCertificateFile>(p => p.Id);
        }
    }
}
