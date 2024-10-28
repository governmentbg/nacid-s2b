using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexCommit
    {
        public int Id { get; set; }

        public int LotId { get; set; }
        public ComplexLot Lot { get; set; }
        public int? State { get; set; }
        public int CommitUserId { get; set; }

        public ComplexBasicPart ComplexBasicPart { get; set; }
        public ComplexCorrespondencePart ComplexCorrespondencePart { get; set; }

        public List<ComplexOrganizationPart> OrganizationParts { get; set; } = new List<ComplexOrganizationPart>();
    }

    public class ComplexCommitConfiguration : IEntityTypeConfiguration<ComplexCommit>
    {
        public void Configure(EntityTypeBuilder<ComplexCommit> builder)
        {
            builder.HasOne(e => e.ComplexBasicPart)
                .WithOne()
                .HasForeignKey<ComplexBasicPart>();

            builder.HasOne(e => e.ComplexCorrespondencePart)
                .WithOne()
                .HasForeignKey<ComplexCorrespondencePart>();

            builder.HasMany(e => e.OrganizationParts)
                .WithOne()
                .HasForeignKey(e => e.CommitId);

            builder.HasOne(e => e.Lot)
                .WithMany()
                .HasForeignKey(e => e.LotId);
        }
    }
}
