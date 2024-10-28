using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.ApproveRegistrations.Base;

namespace Sc.Models.Entities.ApproveRegistrations
{
    public class ApproveRegistration : BaseApproveRegistration
    {
        public ApproveRegistrationFile File { get; set; }

        [SkipUpdate]
        public List<ApproveRegistrationHistory> ApproveRegistrationHistories { get; set; } = new List<ApproveRegistrationHistory>();
    }

    public class ApproveRegistrationConfiguration : IEntityTypeConfiguration<ApproveRegistration>
    {
        public void Configure(EntityTypeBuilder<ApproveRegistration> builder)
        {
            builder.HasMany(e => e.ApproveRegistrationHistories)
                .WithOne()
                .HasForeignKey(e => e.ApproveRegistrationId);

            builder.Property(e => e.DeclinedNote)
                .HasMaxLength(500);
        }
    }
}
