using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Nomenclatures.SmartSpecializations;
using Sc.Models.Entities.Nomenclatures.SmartSpecializations;
using Sc.Models.FilterDtos.Nomenclatures.SmartSpecializations;
using Sc.Repositories.Nomenclatures.SmartSpecializations;
using Sc.Services.Nomenclatures.SmartSpecializations;
using Server.Controllers.Base.Nomenclatures;

namespace Server.Controllers.Nomenclatures.SmartSpecializations
{
    [ApiController]
    [Route("api/nomenclatures/smartSpecializations")]
    public class SmartSpecializationController : NomenclatureSearchController<SmartSpecialization, SmartSpecializationDto, SmartSpecializationDto, SmartSpecializationFilterDto, ISmartSpecializationRepository, SmartSpecializationService>
    {
        public SmartSpecializationController(
            SmartSpecializationService smartSpecializationService
            ) : base(smartSpecializationService)
        {
        }
    }
}
