using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;

namespace Sc.Models.Entities.Suppliers.Junctions
{
    public class SupplierOfferingTeam : EntityVersion
    {
        public int SupplierOfferingId { get; set; }
        [Skip]
        public SupplierOffering SupplierOffering { get; set; }

        public int SupplierTeamId { get; set; }
        [Skip]
        public SupplierTeam SupplierTeam { get; set; }
    }

    public class SupplieOfferingTeamConfiguration : IEntityTypeConfiguration<SupplierOfferingTeam>
    {
        public void Configure(EntityTypeBuilder<SupplierOfferingTeam> builder)
        {
            builder.HasIndex(e => new { e.SupplierOfferingId, e.SupplierTeamId })
                .IsUnique();
        }
    }
}
