using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Institutions;

namespace Sc.Models.Entities.Nomenclatures.Institutions
{
    public class Institution : NomenclatureHierarchy
    {
        public int LotNumber { get; set; }

        [Skip]
        public Institution Parent { get; set; }
        [Skip]
        public Institution Root { get; set; }

        public string Uic { get; set; }
        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

        public OrganizationType? OrganizationType { get; set; }
        public OwnershipType? OwnershipType { get; set; }

        public int? SettlementId { get; set; }
        [Skip]
        public Settlement Settlement { get; set; }
        public int? MunicipalityId { get; set; }
        [Skip]
        public Municipality Municipality { get; set; }
        public int? DistrictId { get; set; }
        [Skip]
        public District District { get; set; }
        public string Address { get; set; }
        public string AddressAlt { get; set; }
        public string WebPageUrl { get; set; }
        public bool IsResearchUniversity { get; set; }

        [Skip]
        public List<Institution> Children { get; set; } = new List<Institution>();

        [Skip]
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }

    public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> builder)
        {
            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId);

            builder.HasOne(e => e.Root)
                .WithMany()
                .HasForeignKey(e => e.RootId);
        }
    }
}
