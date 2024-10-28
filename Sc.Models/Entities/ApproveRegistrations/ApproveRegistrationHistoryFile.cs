using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.ApproveRegistrations
{
    public class ApproveRegistrationHistoryFile : ScAttachedFile
    {
        [Skip]
        public ApproveRegistrationHistory ApproveRegistrationHistory { get; set; }
    }

    public class ApproveRegistrationHistoryFileConfiguration : IEntityTypeConfiguration<ApproveRegistrationHistoryFile>
    {
        public void Configure(EntityTypeBuilder<ApproveRegistrationHistoryFile> builder)
        {
            builder.HasOne(p => p.ApproveRegistrationHistory)
                   .WithOne(a => a.File)
                   .HasForeignKey<ApproveRegistrationHistoryFile>(p => p.Id);
        }
    }
}
