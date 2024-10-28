using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Base.Nomenclatures;

namespace Sc.Services.Nomenclatures.Settlements
{
    public class DistrictService : NomenclatureService<District, DistrictDto, DistrictFilterDto, IDistrictRepository>
    {
        public DistrictService(
            IMapper mapper,
            IDistrictRepository districtRepository,
            DomainValidatorService domainValidatorService
            ) : base(mapper, districtRepository, domainValidatorService)
        {
        }
    }
}
