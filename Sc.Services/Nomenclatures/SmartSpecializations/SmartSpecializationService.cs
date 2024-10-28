using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Nomenclatures.SmartSpecializations;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Models.FilterDtos.Nomenclatures.SmartSpecializations;
using Sc.Repositories.Nomenclatures.SmartSpecializations;
using Sc.Services.Base.Nomenclatures;

namespace Sc.Services.Nomenclatures.SmartSpecializations
{
    public class SmartSpecializationService : NomenclatureService<SmartSpecialization, SmartSpecializationDto, SmartSpecializationFilterDto, ISmartSpecializationRepository>
    {
        public SmartSpecializationService(
            IMapper mapper,
            ISmartSpecializationRepository smartSpecializationRepository,
            DomainValidatorService domainValidatorService
            ) : base(mapper, smartSpecializationRepository, domainValidatorService)
        {
        }
    }
}
