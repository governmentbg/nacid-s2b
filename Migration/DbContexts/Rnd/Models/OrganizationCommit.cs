using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace NacidScMigration.DbContexts.Rnd.Models
{
	public class OrganizationCommit
	{
		public int Id { get; set; }

		public int LotId { get; set; }
		public OrganizationLot Lot { get; set; }
		public int? State { get; set; }
		public DateTime? CommitDate { get; set; }
		public int CommitUserId { get; set; }

		public OrganizationBasicPart OrganizationBasicPart { get; set; }
		public OrganizationCorrespondencePart OrganizationCorrespondencePart { get; set; }
		public OrganizationFinancingInformationPart OrganizationFinancingInformationPart { get; set; }
	}

	public class OrganizationCommitConfiguration : IEntityTypeConfiguration<OrganizationCommit>
	{
		public void Configure(EntityTypeBuilder<OrganizationCommit> builder)
		{
			builder.HasOne(e => e.OrganizationBasicPart)
				.WithOne()
				.HasForeignKey<OrganizationBasicPart>();

			builder.HasOne(e => e.OrganizationCorrespondencePart)
				.WithOne()
				.HasForeignKey<OrganizationCorrespondencePart>();

			builder.HasOne(e => e.OrganizationFinancingInformationPart)
				.WithOne()
				.HasForeignKey<OrganizationFinancingInformationPart>();

			builder.HasOne(e => e.Lot)
				.WithMany()
				.HasForeignKey(e => e.LotId);
		}
	}
}
