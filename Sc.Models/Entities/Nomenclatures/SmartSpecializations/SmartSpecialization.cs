using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sc.Models.Attributes;
using Sc.Models.Entities.Base.Nomenclatures;

namespace Sc.Models.Entities.Nomenclatures.SmartSpecializations
{
    public class SmartSpecialization : NomenclatureHierarchy
    {
        [Skip]
        public SmartSpecialization Parent { get; set; }
        [Skip]
        public SmartSpecialization Root { get; set; }

        public string CodeNumber { get; set; }
    }

    public class SmartSpecializationConfiguration : IEntityTypeConfiguration<SmartSpecialization>
    {
        public void Configure(EntityTypeBuilder<SmartSpecialization> builder)
        {
            builder.HasOne(e => e.Parent)
                .WithMany()
                .HasForeignKey(e => e.ParentId);

            builder.HasOne(e => e.Root)
                .WithMany()
                .HasForeignKey(e => e.RootId);

            builder.HasIndex(e => e.CodeNumber)
                .IsUnique();
        }
    }
}
