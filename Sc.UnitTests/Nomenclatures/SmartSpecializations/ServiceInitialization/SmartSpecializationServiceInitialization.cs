using Infrastructure.DomainValidation;
using Moq;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Repositories.Nomenclatures.SmartSpecializations;
using Sc.Services.Nomenclatures.SmartSpecializations;
using Sc.UnitTests.Common;

namespace Sc.UnitTests.Nomenclatures.SmartSpecializations.ServiceInitialization
{
    public static class SmartSpecializationServiceInitialization
    {
        public static SmartSpecializationService CreateService(ScDbContext context)
        {
            var autoMapper = MockAutoMapper.CreateAutoMapper();
            var smartSpecializationRepository = new SmartSpecializationRepository(context);
            var domainValidatorService = new Mock<DomainValidatorService>();
            return new SmartSpecializationService(autoMapper, smartSpecializationRepository, domainValidatorService.Object);
        }

        public static void SeedSmartSpecialization(this ScDbContext context)
        {
            context.SmartSpecializations.Add(new SmartSpecialization
            {
                Id = 1,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "1.",
                Name = "Информатика и ИКТ",
                NameAlt = "SmartSpecializationTest1",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                ParentId = 1,
                RootId = 1
            });

            context.SmartSpecializations.Add(new SmartSpecialization
            {
                Id = 2,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "2.",
                Name = "услугаТест2",
                NameAlt = "SmartSpecializationTest2",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                ParentId = 1,
                RootId = 1
            });

            context.SmartSpecializations.Add(new SmartSpecialization
            {
                Id = 3,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "3.",
                Name = "услугаТест3",
                NameAlt = "SmartSpecializationTest3",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
                ParentId = 1,
                RootId = 1
            });

            context.SaveChanges();
        }
    }
}
