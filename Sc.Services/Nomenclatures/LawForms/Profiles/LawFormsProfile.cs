using AutoMapper;
using Sc.Models.Dtos.Nomenclatures;
using Sc.Models.Entities.Nomenclatures;

namespace Sc.Services.Nomenclatures.LawForms.Profiles
{
    public class LawFormsProfile : Profile
    {
        public LawFormsProfile()
        {
            CreateMap<LawForm, LawFormDto>();

            CreateMap<LawFormDto, LawForm>();
        }
    }
}
