using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Nomenclatures.Settlements;
using Sc.UnitTests.Common;
using Infrastructure.DomainValidation;
using Moq;

namespace Sc.UnitTests.Nomenclatures.Settlements.ServiceInitialization
{
    public static class SettlementServiceInitialization
    {
        public static SettlementService CreateService(ScDbContext context)
        {
            var autoMapper = MockAutoMapper.CreateAutoMapper();
            var settlementRepository = new SettlementRepository(context);
            var domainValidatorService = new Mock<DomainValidatorService>();
            return new SettlementService(autoMapper, settlementRepository, domainValidatorService.Object);
        }

        public static void SeedSettlements(this ScDbContext context)
        {
            context.Settlements.Add(new Settlement
            {
                Id = 1,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "00014",
                Name = "НаселеноМястоТест",
                NameAlt = "SettlementTest",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                SettlementName = "НаселеноМястоИмеТест",
                Altitude = "5",
                Category = "1",
                District = new District(),
                DistrictCode = "123",
                DistrictCode2 = "1234",
                DistrictId = 1,
                IsDistrict = true,
                MayoraltyCode = "123",
                Municipality = new Municipality(),
                MunicipalityId = 1,
                MunicipalityCode = "123",
                MunicipalityCode2 = "123",
                TypeCode = "123",
                TypeName = "name"

            });

            context.Settlements.Add(new Settlement
            {
                Id = 2,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "00015",
                Name = "НаселеноМястоТест2",
                NameAlt = "SettlementTest2",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                SettlementName = "НаселеноМястоИмеТест2",
                Altitude = "52",
                Category = "1",
                District = new District(),
                DistrictCode = "1232",
                DistrictCode2 = "12342",
                DistrictId = 1,
                IsDistrict = true,
                MayoraltyCode = "1232",
                Municipality = new Municipality(),
                MunicipalityId = 1,
                MunicipalityCode2 = "1232",
                TypeCode = "1232",
                TypeName = "name2"
            });

            context.Settlements.Add(new Settlement
            {
                Id = 3,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "00016",
                Name = "НаселеноМястоТест3",
                NameAlt = "SettlementTest3",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                SettlementName = "НаселеноМястоИмеТест3",
                Altitude = "53",
                Category = "1",
                District = new District(),
                DistrictCode = "1233",
                DistrictCode2 = "12343",
                DistrictId = 1,
                IsDistrict = true,
                MayoraltyCode = "1233",
                Municipality = new Municipality(),
                MunicipalityId = 1,
                MunicipalityCode2 = "1233",
                TypeCode = "1233",
                TypeName = "name3"
            });

            context.SaveChanges();
        }
    }
}
