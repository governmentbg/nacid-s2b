using Infrastructure.DomainValidation;
using Moq;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Nomenclatures.Settlements;
using Sc.UnitTests.Common;

namespace Sc.UnitTests.Nomenclatures.Districts.ServiceInitialization
{
    public static class DistrictServiceInitialization
    {
        public static DistrictService CreateService(ScDbContext context)
        {
            var autoMapper = MockAutoMapper.CreateAutoMapper();
            var districtRepository = new DistrictRepository(context);
            var domainValidatorService = new Mock<DomainValidatorService>();
            return new DistrictService(autoMapper, districtRepository, domainValidatorService.Object);
        }

        public static void SeedDistricts(this ScDbContext context)
        {
            context.Districts.Add(new District
            {
                Id = 1,
                Code2 = "01",
                SecondLevelRegionCode = "BG41",
                MainSettlementCode = "04279",
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "BLG",
                Name = "ОбластТест",
                NameAlt = "DistrictTest",
                Description = null,
                DescriptionAlt = null,
                IsActive = true
            });

            context.Districts.Add(new District
            {
                Id = 2,
                Code2 = "02",
                SecondLevelRegionCode = "BG42",
                MainSettlementCode = "04280",
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "BGG",
                Name = "ОбластТест2",
                NameAlt = "DistrictTest2",
                Description = null,
                DescriptionAlt = null,
                IsActive = true
            });

            context.Districts.Add(new District
            {
                Id = 3,
                Code2 = "03",
                SecondLevelRegionCode = "BG43",
                MainSettlementCode = "04281",
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "BBB",
                Name = "ОбластТест3",
                NameAlt = "DistrictTest3",
                Description = null,
                DescriptionAlt = null,
                IsActive = true
            });

            context.SaveChanges();
        }
    }
}
