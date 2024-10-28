using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Entities.Suppliers.Junctions;
using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Entities.Suppliers
{
    public class SupplierOffering : EntityVersion
    {
        public int SupplierId { get; set; }
        [Skip]
        public Supplier Supplier { get; set; }

        [Skip]
        public string Code { get; set; }

        public OfferingType OfferingType { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public int SettlementId { get; set; }
        [Skip]
        public Settlement Settlement { get; set; }
        public int MunicipalityId { get; set; }
        [Skip]
        public Municipality Municipality { get; set; }
        public int DistrictId { get; set; }
        [Skip]
        public District District { get; set; }
        public string Address { get; set; }
        public string WebPageUrl { get; set; }

        public bool IsActive { get; set; }

        public List<SupplierOfferingFile> Files { get; set; } = new List<SupplierOfferingFile>();

        public List<SupplierOfferingSmartSpecialization> SmartSpecializations { get; set; } = new List<SupplierOfferingSmartSpecialization>();
        public List<SupplierOfferingTeam> SupplierOfferingTeams { get; set; } = new List<SupplierOfferingTeam>();
        public List<SupplierOfferingEquipment> SupplierOfferingEquipment { get; set; } = new List<SupplierOfferingEquipment>();
    }

    public class SupplierOfferingConfiguration : IEntityTypeConfiguration<SupplierOffering>
    {
        public void Configure(EntityTypeBuilder<SupplierOffering> builder)
        {
            builder.Property(e => e.Code)
                .IsRequired();

            builder.HasIndex(e => e.Code)
                .IsUnique();

            builder.Property(e => e.Name)
                .HasMaxLength(200);

            builder.Property(e => e.ShortDescription)
                .HasMaxLength(250);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.WebPageUrl)
                .HasMaxLength(100);

            builder.Property(e => e.Address)
                .HasMaxLength(250);
        }
    }
}
