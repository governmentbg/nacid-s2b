using Microsoft.EntityFrameworkCore;
using Migration.DbContexts.Rnd.Models;
using NacidScMigration.DbContexts.Rnd.Models;

namespace Migration.DbContexts.Rnd
{
    public class NacidRndContext : DbContext
    {
        public DbSet<ComplexBasic> ComplexBasics { get; set; }
        public DbSet<ComplexBasicPart> ComplexBasicParts { get; set; }
        public DbSet<ComplexCorrespondence> ComplexCorrespondences { get; set; }
        public DbSet<ComplexCorrespondencePart> ComplexCorrespondenceParts { get; set; }
        public DbSet<ComplexOrganization> ComplexOrganizations { get; set; }
        public DbSet<ComplexOrganizationPart> ComplexOrganizationParts { get; set; }
        public DbSet<ComplexCommit> ComplexCommits { get; set; }
        public DbSet<ComplexLot> ComplexLots { get; set; }

        public DbSet<FinancingOrganizationBasic> FinancingOrganizationBasics { get; set; }
        public DbSet<FinancingOrganizationBasicPart> FinancingOrganizationBasicParts { get; set; }
        public DbSet<FinancingOrganizationCommit> FinancingOrganizationCommits { get; set; }

        public DbSet<OrganizationBasic> OrganizationBasics { get; set; }
        public DbSet<OrganizationBasicPart> OrganizationBasicParts { get; set; }
        public DbSet<OrganizationCommit> OrganizationCommits { get; set; }
        public DbSet<OrganizationCorrespondence> OrganizationCorrespondences { get; set; }
        public DbSet<OrganizationCorrespondencePart> OrganizationCorrespondenceParts { get; set; }
        public DbSet<OrganizationFinancingInformation> OrganizationFinancingInformations { get; set; }
        public DbSet<OrganizationFinancingInformationPart> OrganizationFinancingInformationParts { get; set; }
        public DbSet<OrganizationLot> OrganizationLots { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }

        public NacidRndContext(DbContextOptions<NacidRndContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyConfigurations(modelBuilder);
        }

        protected void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }

            modelBuilder.ApplyConfiguration(new OrganizationCommitConfiguration());
            modelBuilder.ApplyConfiguration(new ComplexCommitConfiguration());
            modelBuilder.ApplyConfiguration(new FinancingOrganizationCommitConfiguration());
        }
    }
}
