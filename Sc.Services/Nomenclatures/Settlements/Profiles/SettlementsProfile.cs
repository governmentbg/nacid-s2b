using AutoMapper;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.Settlements;

namespace Sc.Services.Nomenclatures.Settlements.Profiles
{
    public class SettlementsProfile : Profile
    {
        public SettlementsProfile()
        {
            CreateMap<Settlement, SettlementDto>();
            CreateMap<Municipality, MunicipalityDto>();
            CreateMap<District, DistrictDto>();

            CreateMap<SettlementDto, Settlement>();
            CreateMap<MunicipalityDto, Municipality>();
            CreateMap<DistrictDto, District>();
        }
    }
}
