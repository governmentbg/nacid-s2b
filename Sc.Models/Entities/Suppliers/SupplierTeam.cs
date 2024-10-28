using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Suppliers.Junctions;

namespace Sc.Models.Entities.Suppliers
{
    public class SupplierTeam : EntityVersion
    {
        public int SupplierId { get; set; }
        [Skip]
        public Supplier Supplier { get; set; }

        [SkipUpdate]
        public int UserId { get; set; }
        [SkipUpdate]
        public string UserName { get; set; }

        public string AcademicRankDegree { get; set; }
        public string Position { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int? RasLotId { get; set; }
        public int? RasLotNumber { get; set; }
        public string RasPortalUrl { get; set; }

        public bool IsActive { get; set; }

        public List<SupplierOfferingTeam> SupplierOfferingTeams { get; set; } = new List<SupplierOfferingTeam>();
    }

    public class SupplierTeamConfiguration : IEntityTypeConfiguration<SupplierTeam>
    {
        public void Configure(EntityTypeBuilder<SupplierTeam> builder)
        {
            builder.HasIndex(e => new { e.UserId, e.SupplierId })
                .IsUnique();

            builder.Property(e => e.Position)
                .HasMaxLength(50);

            builder.Property(e => e.AcademicRankDegree)
                .HasMaxLength(50);

            builder.Property(e => e.FirstName)
                .HasMaxLength(50);

            builder.Property(e => e.MiddleName)
                .HasMaxLength(50);

            builder.Property(e => e.LastName)
                .HasMaxLength(50);

            builder.Property(e => e.Name)
                .HasMaxLength(250);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(18);
        }
    }
}
