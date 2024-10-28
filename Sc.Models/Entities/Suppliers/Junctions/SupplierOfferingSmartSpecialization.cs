using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Entities.Suppliers.Junctions
{
    public class SupplierOfferingSmartSpecialization : EntityVersion
    {
        public int SupplierOfferingId { get; set; }
        [Skip]
        public SupplierOffering SupplierOffering { get; set; }

        public int SmartSpecializationId { get; set; }
        [Skip]
        public SmartSpecialization SmartSpecialization { get; set; }

        public SupplierOfferingSmartSpecializationType Type { get; set; }
    }

    public class SupplierOfferingSmartSpecializationConfiguration : IEntityTypeConfiguration<SupplierOfferingSmartSpecialization>
    {
        public void Configure(EntityTypeBuilder<SupplierOfferingSmartSpecialization> builder)
        {
            builder.HasIndex(e => new { e.SupplierOfferingId, e.SmartSpecializationId })
                .IsUnique();
        }
    }
}
