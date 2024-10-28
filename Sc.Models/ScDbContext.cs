using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sc.Models.Entities.ApproveRegistrations;
using Sc.Models.Entities.Companies;
using Sc.Models.Entities.Emails;
using Sc.Models.Entities.Nomenclatures;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Entities.SchemaVersions;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Entities.Suppliers.Junctions;
using Sc.Models.Entities.VoucherRequests;
using System.Reflection;

namespace Sc.Models
{
    public class ScDbContext : DbContext
    {
        #region Nomenclatures
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<LawForm> LawForms { get; set; }
        public DbSet<SmartSpecialization> SmartSpecializations { get; set; }
        public DbSet<Complex> Complexes { get; set; }
        public DbSet<ComplexOrganization> ComplexOrganizations { get; set; }
        #endregion

        #region Suppliers
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierRepresentative> SupplierRepresentatives { get; set; }
        public DbSet<SupplierTeam> SupplierTeams { get; set; }
        public DbSet<SupplierOffering> SupplierOfferings { get; set; }
        public DbSet<SupplierOfferingCounter> SupplierOfferingCounters { get; set; }
        public DbSet<SupplierOfferingFile> SupplierOfferingFiles { get; set; }
        public DbSet<SupplierOfferingTeam> SupplierOfferingTeams { get; set; }
        public DbSet<SupplierOfferingEquipment> SupplierOfferingEquipment { get; set; }
        public DbSet<SupplierOfferingSmartSpecialization> SupplierOfferingSmartSpecializations { get; set; }
        public DbSet<SupplierEquipment> SupplierEquipment { get; set; }
        public DbSet<SupplierEquipmentFile> SupplierEquipmentFiles { get; set; }
        #endregion

        #region Companies
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyRepresentative> CompanyRepresentatives { get; set; }
        public DbSet<CompanyAdditional> CompanyAdditionals { get; set; }
        #endregion

        #region ApproveRegistrations
        public DbSet<ApproveRegistration> ApproveRegistrations { get; set; }
        public DbSet<ApproveRegistrationFile> ApproveRegistrationFiles { get; set; }
        public DbSet<ApproveRegistrationHistory> ApproveRegistrationHistories { get; set; }
        public DbSet<ApproveRegistrationHistoryFile> ApproveRegistrationHistoryFiles { get; set; }
        #endregion

        #region VoucherRequests
        public DbSet<VoucherRequest> VoucherRequests { get; set; }
        public DbSet<VoucherRequestCommunication> VoucherRequestCommunications { get; set; }
        public DbSet<VoucherRequestNotification> VoucherRequestNotifications { get; set; }
        #endregion

        #region ReceivedVouchers
        public DbSet<ReceivedVoucher> ReceivedVouchers { get; set; }
        public DbSet<ReceivedVoucherFile> ReceivedVoucherFiles { get; set; }
        public DbSet<ReceivedVoucherCertificate> ReceivedVoucherCertificates { get; set; }
        public DbSet<ReceivedVoucherCertificateFile> ReceivedVoucherCertificateFiles { get; set; }
        public DbSet<ReceivedVoucherHistory> ReceivedVoucherHistories { get; set; }
        public DbSet<ReceivedVoucherHistoryFile> ReceivedVoucherHistoryFiles { get; set; }
        public DbSet<ReceivedVoucherCommunication> ReceivedVoucherCommunications { get; set; }
        public DbSet<ReceivedVoucherNotification> ReceivedVoucherNotifications { get; set; }
        #endregion

        #region Emails
        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        #endregion

        public ScDbContext(DbContextOptions<ScDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyConfigurations(modelBuilder);
            DisableCascadeDelete(modelBuilder);
            ConfigurePgSqlNameMappings(modelBuilder);

            modelBuilder.Entity<SchemaVersion>()
                .ToTable("schemaversions");
        }

        protected void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                   .Where(t => t.GetInterfaces().Any(gi =>
                       gi.IsGenericType
                       && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                   .ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }

        protected void DisableCascadeDelete(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership
                    && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);
        }

        protected void ConfigurePgSqlNameMappings(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Configure pgsql table names convention.
                entity.SetTableName(entity.ClrType.Name.ToLower());

                // Configure pgsql column names convention.
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }
        }
    }
}
