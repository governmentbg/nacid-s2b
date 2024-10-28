using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Nomenclatures;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Enums.Companies;

namespace Sc.Models.Entities.Companies
{
    public class Company : EntityVersion
    {
        public string Uic { get; set; }
        public CompanyType Type { get; set; }

        public int LawFormId { get; set; }
        [Skip]
        public LawForm LawForm { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }

        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

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
        public string AddressAlt { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [SkipUpdate]
        public CompanyRepresentative Representative { get; set; }

        [Skip]
        public CompanyAdditional CompanyAdditional { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsRegistryAgency { get; set; }
    }

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(e => e.Uic)
                .IsUnique();

            builder.Property(e => e.Uic)
                .HasMaxLength(13);

            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.Address)
                .HasMaxLength(250);

            builder.Property(e => e.Email)
                .HasMaxLength(50);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(18);
        }
    }
}
