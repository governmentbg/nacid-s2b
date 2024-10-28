using Sc.Models;
using Sc.UnitTests.Common;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Infrastructure.DomainValidation;
using Moq;

namespace Sc.UnitTests.Nomenclatures.Municipalities.ServiceInitialization
{
    public static class MunicipalityServiceInitialization
    {
        public static MunicipalityService CreateService(ScDbContext context)
        {
            var autoMapper = MockAutoMapper.CreateAutoMapper();
            var municipalityRepository = new MunicipalityRepository(context);
            var domainValidatorService = new Mock<DomainValidatorService>();
            return new MunicipalityService(autoMapper, municipalityRepository, domainValidatorService.Object);
        }

        public static void SeedMunicipalities(this ScDbContext context)
        {
            context.Municipalities.Add(new Municipality
            {
                Id = 1,
                Code2 = "0102",
                MainSettlementCode = "04279",
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "BGS03",
                Name = "ОбщинаТест",
                NameAlt = "MunicipalityTest",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                DistrictId = 1,
                Category = "1",
                District = new District()
            });

            context.Municipalities.Add(new Municipality
            {
                Id = 2,
                Code2 = "0201",
                MainSettlementCode = "04280",
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "BGS02",
                Name = "ОбщинаТест2",
                NameAlt = "MunicipalityTest2",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                DistrictId = 1,
                Category = "1",
                District = new District()
            });

            context.Municipalities.Add(new Municipality
            {
                Id = 3,
                Code2 = "0303",
                MainSettlementCode = "04281",
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "BGS01",
                Name = "ОбщинаТест3",
                NameAlt = "MunicipalityTest3",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                DistrictId = 1,
                Category = "1",
                District = new District()
            });

            context.SaveChanges();
        }
    }
}
