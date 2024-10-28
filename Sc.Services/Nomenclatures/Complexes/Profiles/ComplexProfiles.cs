using AutoMapper;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Entities.Nomenclatures.Complexes;

namespace Sc.Services.Nomenclatures.Complexes.Profiles
{
    public class ComplexProfiles : Profile
    {
        public ComplexProfiles()
        {
            CreateMap<Complex, ComplexDto>();

            CreateMap<ComplexDto, Complex>();
        }
    }
}
