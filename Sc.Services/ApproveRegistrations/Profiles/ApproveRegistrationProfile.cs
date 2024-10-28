using AutoMapper;
using Newtonsoft.Json;
using Sc.Models.Dtos.ApproveRegistrations;
using Sc.Models.Dtos.ApproveRegistrations.Search;
using Sc.Models.Dtos.Auth;
using Sc.Models.Entities.ApproveRegistrations;

namespace Sc.Services.ApproveRegistrations.Profiles
{
    public class ApproveRegistrationProfile : Profile
    {
        public ApproveRegistrationProfile()
        {
            CreateMap<ApproveRegistration, ApproveRegistrationSearchDto>()
                 .ForMember(dest => dest.SignUpDto, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<SignUpDto>(src.JsonSignUpDto)));
            CreateMap<ApproveRegistrationHistory, ApproveRegistrationHistorySearchDto>()
                 .ForMember(dest => dest.SignUpDto, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<SignUpDto>(src.JsonSignUpDto)));
            CreateMap<ApproveRegistrationFile, ApproveRegistrationFileDto>();
            CreateMap<ApproveRegistrationHistoryFile, ApproveRegistrationHistoryFileDto>();

            CreateMap<ApproveRegistrationSearchDto, ApproveRegistration>();
            CreateMap<ApproveRegistrationHistorySearchDto, ApproveRegistrationHistory>();
            CreateMap<ApproveRegistrationFileDto, ApproveRegistrationFile>();
            CreateMap<ApproveRegistrationHistoryFileDto, ApproveRegistrationHistoryFile>();
        }
    }
    

}
