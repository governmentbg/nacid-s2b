using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.FileStorages;

namespace Sc.Models.Entities.ApproveRegistrations
{
    public class ApproveRegistrationFile : ScAttachedFile
    {
        [Skip]
        public ApproveRegistration ApproveRegistration { get; set; }
    }

    public class ApproveRegistrationFileConfiguration : IEntityTypeConfiguration<ApproveRegistrationFile>
    {
        public void Configure(EntityTypeBuilder<ApproveRegistrationFile> builder)
        {
            builder.HasOne(p => p.ApproveRegistration)
                   .WithOne(a => a.File)
                   .HasForeignKey<ApproveRegistrationFile>(p => p.Id);
        }
    }
}
