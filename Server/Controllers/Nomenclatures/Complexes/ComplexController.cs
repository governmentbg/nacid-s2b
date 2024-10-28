using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.FilterDtos.Nomenclatures.Complexes;
using Sc.Repositories.Nomenclatures.Complexes;
using Sc.Services.Nomenclatures.Complexes;
using Server.Controllers.Base.Nomenclatures;

namespace Server.Controllers.Nomenclatures.Complexes
{
    [ApiController]
    [Route("api/nomenclatures/complexes")]
    public class ComplexController : NomenclatureSearchController<Complex, ComplexDto, ComplexDto, ComplexFilterDto, IComplexRepository, ComplexService>
    {
        public ComplexController(
            ComplexService complexService
            ) : base(complexService)
        {
        }
    }
}
