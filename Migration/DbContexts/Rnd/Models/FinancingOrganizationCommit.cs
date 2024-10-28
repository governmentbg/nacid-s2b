using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Migration.DbContexts.Rnd.Models
{
    public class FinancingOrganizationCommit
    {
        public int Id { get; set; }

        public int LotId { get; set; }
        public int? State { get; set; }
        public int CommitUserId { get; set; }

        public FinancingOrganizationBasicPart FinancingOrganizationBasicPart { get; set; }
    }

    public class FinancingOrganizationCommitConfiguration : IEntityTypeConfiguration<FinancingOrganizationCommit>
    {
        public void Configure(EntityTypeBuilder<FinancingOrganizationCommit> builder)
        {
            builder.HasOne(e => e.FinancingOrganizationBasicPart)
                .WithOne()
                .HasForeignKey<FinancingOrganizationBasicPart>();
        }
    }
}
