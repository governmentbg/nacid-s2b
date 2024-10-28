using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherFile : ScAttachedFile
    {
        [Skip]
        public ReceivedVoucher ReceivedVoucher { get; set; }
    }

    public class ReceivedVoucherFileConfiguration : IEntityTypeConfiguration<ReceivedVoucherFile>
    {
        public void Configure(EntityTypeBuilder<ReceivedVoucherFile> builder)
        {
            builder.HasOne(p => p.ReceivedVoucher)
                   .WithOne(a => a.File)
                   .HasForeignKey<ReceivedVoucherFile>(p => p.Id);
        }
    }
}
