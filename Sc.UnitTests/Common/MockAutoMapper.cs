using AutoMapper;
using Sc.Services.Nomenclatures.LawForms.Profiles;
using Sc.Services.Nomenclatures.Settlements.Profiles;
using Sc.Services.Nomenclatures.SmartSpecializations.Profiles;

namespace Sc.UnitTests.Common
{
    public class MockAutoMapper
    {
        public static IMapper CreateAutoMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SettlementsProfile());
                cfg.AddProfile(new LawFormsProfile());
                cfg.AddProfile(new SmartSpecializationsProfile());
            });

            return mockMapper.CreateMapper();
        }
    }
}
