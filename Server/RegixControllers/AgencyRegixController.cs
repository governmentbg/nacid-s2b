using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Companies;
using Sc.Services.AgencyRegix;

namespace Server.RegixControllers
{
    [ApiController]
    [Route("api/regix/agency")]
    public class AgencyRegixController : ControllerBase
    {
        private readonly AgencyRegixService agencyRegixService;

        public AgencyRegixController(
            AgencyRegixService agencyRegixService
        )
        {
            this.agencyRegixService = agencyRegixService;
        }

        [HttpGet("company/{uic}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyFromAgencyRegix([FromRoute] string uic, CancellationToken cancellationToken)
        {
            var companyDto = await agencyRegixService.GetCompanyFromAgencyRegix(uic, cancellationToken);

            return Ok(companyDto);
        }
    }
}
