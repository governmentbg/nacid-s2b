using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Entities.Base;
using Sc.Models.Attributes;

namespace Sc.Models.Entities.Companies
{
    public class CompanyAdditional : EntityVersion
    {
        [Skip]
        public Company Company { get; set; }

        public uint StaffCount { get; set; }
        public decimal AnnualTurnover { get; set; }
        public string WebPage { get; set; }
    }

    public class CompanyAdditionalConfiguration : IEntityTypeConfiguration<CompanyAdditional>
    {
        public void Configure(EntityTypeBuilder<CompanyAdditional> builder)
        {
            builder.HasOne(p => p.Company)
                   .WithOne(a => a.CompanyAdditional)
                   .HasForeignKey<CompanyAdditional>(p => p.Id);

            builder.Property(e => e.WebPage)
                .HasMaxLength(50);
        }
    }
}
