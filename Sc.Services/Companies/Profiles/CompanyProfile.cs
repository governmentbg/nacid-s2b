using AutoMapper;
using Sc.Models.Dtos.Companies;
using Sc.Models.Entities.Companies;

namespace Sc.Services.Companies.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyRepresentative, CompanyRepresentativeDto>();
            CreateMap<CompanyAdditional, CompanyAdditionalDto>();

            CreateMap<CompanyDto, Company>();
            CreateMap<CompanyRepresentativeDto, CompanyRepresentative>();
            CreateMap<CompanyAdditionalDto, CompanyAdditional>();
        }
    }
}
