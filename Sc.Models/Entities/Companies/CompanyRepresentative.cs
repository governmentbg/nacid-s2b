using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base;

namespace Sc.Models.Entities.Companies
{
    public class CompanyRepresentative : EntityVersion
    {
        [Skip]
        public Company Company { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CompanyRepresentativeConfiguration : IEntityTypeConfiguration<CompanyRepresentative>
    {
        public void Configure(EntityTypeBuilder<CompanyRepresentative> builder)
        {
            builder.HasOne(p => p.Company)
                   .WithOne(a => a.Representative)
                   .HasForeignKey<CompanyRepresentative>(p => p.Id);
        }
    }
}
