using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Nomenclatures.Institutions;
using Sc.Services.Nomenclatures.Institutions;

namespace Server.Controllers.Nomenclatures.Institutions
{
    [ApiController]
    [Route("api/nomenclatures/institutions")]
    public class InstitutionController : ControllerBase
    {
        private readonly InstitutionService institutionService;

        public InstitutionController(
            InstitutionService institutionService
            )
        {
            this.institutionService = institutionService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstitutionDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await institutionService.GetById(id, cancellationToken));
        }

        [HttpPost("search")]
        public async Task<ActionResult<SearchResultDto<InstitutionDto>>> GetSearchResultDto([FromBody] InstitutionFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await institutionService.GetSearchResultDto<InstitutionDto>(filter, IncludeType.NavProperties, cancellationToken));
        }

        [HttpGet("{parentId:int}/subordinates")]
        public async Task<ActionResult<List<InstitutionDto>>> GetSubordinates([FromRoute] int parentId, CancellationToken cancellationToken)
        {
            return Ok(await institutionService.GetSubordinates(parentId, cancellationToken));
        }
    }
}
