using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.ReceivedVouchers
{
    public class ReceivedVoucherHistoryFile : ScAttachedFile
    {
        [Skip]
        public ReceivedVoucherHistory ReceivedVoucherHistory { get; set; }
    }

    public class ReceivedVoucherHistoryFileConfiguration : IEntityTypeConfiguration<ReceivedVoucherHistoryFile>
    {
        public void Configure(EntityTypeBuilder<ReceivedVoucherHistoryFile> builder)
        {
            builder.HasOne(p => p.ReceivedVoucherHistory)
                   .WithOne(a => a.File)
                   .HasForeignKey<ReceivedVoucherHistoryFile>(p => p.Id);
        }
    }
}
