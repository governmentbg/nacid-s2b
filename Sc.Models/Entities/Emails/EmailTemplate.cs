using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Entities.Base;

namespace Sc.Models.Entities.Emails
{
    public class EmailTemplate : EntityVersion
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; }
    }

    public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.HasIndex(e => e.Alias)
                .IsUnique();
        }
    }
}
