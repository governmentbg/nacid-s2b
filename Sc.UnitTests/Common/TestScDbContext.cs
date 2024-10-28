using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sc.Models;
using Sc.UnitTests.Nomenclatures.Districts.ServiceInitialization;
using Sc.UnitTests.Nomenclatures.LawForms.ServiceInitialization;
using Sc.UnitTests.Nomenclatures.Municipalities.ServiceInitialization;
using Sc.UnitTests.Nomenclatures.Settlements.ServiceInitialization;
using Sc.UnitTests.Nomenclatures.SmartSpecializations.ServiceInitialization;

namespace Sc.UnitTests.Common
{
    public static class TestScDbContext
    {
        public static DbContextOptions<ScDbContext> GetScDbContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ScDbContext>();
            builder.UseInMemoryDatabase("NacidScTests")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static void SeedFullDatabase(this ScDbContext context)
        {
            context.SeedDistricts();
            context.SeedLawForms();
            context.SeedMunicipalities();
            context.SeedSettlements();
            context.SeedSmartSpecialization();
        }

    }
}
