using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Base.Nomenclatures;

namespace Sc.Services.Nomenclatures.Settlements
{
    public class MunicipalityService : NomenclatureService<Municipality, MunicipalityDto, MunicipalityFilterDto, IMunicipalityRepository>
    {
        public MunicipalityService(
            IMapper mapper,
            IMunicipalityRepository municipalityRepository,
            DomainValidatorService domainValidatorService
            ) : base(mapper, municipalityRepository, domainValidatorService)
        {
        }
    }
}
