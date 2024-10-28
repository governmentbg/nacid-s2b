using AutoMapper;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Entities.Nomenclatures.Institutions;

namespace Sc.Services.Nomenclatures.Institutions.Profiles
{
    public class InstitutionProfile : Profile
    {
        public InstitutionProfile()
        {
            CreateMap<Institution, InstitutionDto>();

            CreateMap<InstitutionDto, Institution>();
        }
    }
}
