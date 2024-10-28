using AutoMapper;
using Sc.Models.Dtos.Nomenclatures.SmartSpecializations;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;

namespace Sc.Services.Nomenclatures.SmartSpecializations.Profiles
{
    public class SmartSpecializationsProfile : Profile
    {
        public SmartSpecializationsProfile()
        {
            CreateMap<SmartSpecialization, SmartSpecializationDto>();

            CreateMap<SmartSpecializationDto, SmartSpecialization>();
        }
    }
}
