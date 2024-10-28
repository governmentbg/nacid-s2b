using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Nomenclatures.Settlements;
using Server.Controllers.Base.Nomenclatures;

namespace Server.Controllers.Nomenclatures.Settlements
{
    [ApiController]
    [Route("api/nomenclatures/municipalities")]
    public class MunicipalityController : NomenclatureSearchController<Municipality, MunicipalityDto, MunicipalityDto, MunicipalityFilterDto, IMunicipalityRepository, MunicipalityService>
    {
        public MunicipalityController(
            MunicipalityService municipalityService
            ) : base(municipalityService)
        {
        }
    }
}
