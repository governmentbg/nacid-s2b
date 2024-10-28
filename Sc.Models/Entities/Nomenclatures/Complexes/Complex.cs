using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Complexes;

namespace Sc.Models.Entities.Nomenclatures.Complexes
{
    public class Complex : Nomenclature
    {
        public int LotNumber { get; set; }

        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

        public string CoordinatorPosition { get; set; }
        public string CoordinatorPositionAlt { get; set; }
        public string Category { get; set; }
        public string CategoryAlt { get; set; }
        public string Benefits { get; set; }
        public string BenefitsAlt { get; set; }
        public string ScientificTeam { get; set; }
        public string ScientificTeamAlt { get; set; }

        public AreaOfActivity? AreaOfActivity { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string EuropeanInfrastructure { get; set; }

        public bool IsForeign { get; set; }
        public int? SettlementId { get; set; }
        [Skip]
        public Settlement Settlement { get; set; }
        public int? MunicipalityId { get; set; }
        [Skip]
        public Municipality Municipality { get; set; }
        public int? DistrictId { get; set; }
        [Skip]
        public District District { get; set; }
        public string ForeignSettlement { get; set; }
        public string ForeignSettlementAlt { get; set; }
        public string Address { get; set; }
        public string AddressAlt { get; set; }

        public string WebPageUrl { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        [Skip]
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public List<ComplexOrganization> ComplexOrganizations { get; set; } = new List<ComplexOrganization>();
    }

    public class ComplexConfiguration : IEntityTypeConfiguration<Complex>
    {
        public void Configure(EntityTypeBuilder<Complex> builder)
        {
            builder.HasMany(e => e.ComplexOrganizations)
                .WithOne()
                .HasForeignKey(e => e.ComplexId);
        }
    }
}
