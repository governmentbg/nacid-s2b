using Sc.Models;
using Sc.UnitTests.Common;
using Sc.Services.Nomenclatures.LawForms;
using Sc.Repositories.Nomenclatures;
using Sc.Models.Entities.Nomenclatures;
using Infrastructure.DomainValidation;
using Moq;

namespace Sc.UnitTests.Nomenclatures.LawForms.ServiceInitialization
{
    public static class LawFormServiceInitialization
    {
        public static LawFormService CreateService(ScDbContext context)
        {
            var autoMapper = MockAutoMapper.CreateAutoMapper();
            var lawFormRepository = new LawFormRepository(context);
            var domainValidatorService = new Mock<DomainValidatorService>();
            return new LawFormService(autoMapper, lawFormRepository, domainValidatorService.Object);
        }

        public static void SeedLawForms(this ScDbContext context)
        {
            context.LawForms.Add(new LawForm
            {
                Id = 1,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "АД",
                Name = "ПравнаФормаТест",
                NameAlt = "LawFormTest",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,

            });

            context.LawForms.Add(new LawForm
            {
                Id = 2,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "ООД",
                Name = "ПравнаФормаТест2",
                NameAlt = "LawFormTest2",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
            });

            context.LawForms.Add(new LawForm
            {
                Id = 3,
                Version = 0,
                ViewOrder = 0,
                Alias = null,
                Code = "ЕООД",
                Name = "ПравнаФормаТест3",
                NameAlt = "LawFormTest3",
                Description = null,
                DescriptionAlt = null,
                IsActive = true,
            });

            context.SaveChanges();
        }
    }
}
