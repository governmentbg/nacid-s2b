using AutoMapper;
using Infrastructure.Permissions.Constants;
using Sc.Models.Dtos.Auth;
using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Dtos.Sso;
using Sc.Models.Dtos.Suppliers;

namespace Sc.Services.Auth.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<InstitutionDto, SsoOrganizationalUnitDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Alias, opt => opt.UseDestinationValue());

            CreateMap<ComplexDto, SsoOrganizationalUnitDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => OrganizationalUnitConstants.ComplexAlias));

            CreateMap<CompanyDto, SsoOrganizationalUnitDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<SsoUserDto, SupplierRepresentativeDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.UserInfo.FullName));

            CreateMap<SsoUserDto, CompanyRepresentativeDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.UserInfo.FullName));

            CreateMap<SignUpDto, SupplierDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.SupplierType))
                .ForPath(dest => dest.InstitutionId, opt => opt.MapFrom(src => src.Institution.Id))
                .ForPath(dest => dest.ComplexId, opt => opt.MapFrom(src => src.Complex.Id));

        }
    }
}
