using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Reports.BgMap;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Reports.BgMap;

namespace Server.Controllers.Reports
{
    [ApiController]
    [Route("api/report/bgMap")]
    public class BgMapReportController : ControllerBase
    {
        private readonly BgMapReportService bgMapReportService;

        public BgMapReportController(
            BgMapReportService bgMapReportService
            )
        {
            this.bgMapReportService = bgMapReportService;
        }

        [HttpPost]
        public async Task<ActionResult<List<BgMapReportDto>>> GetBgMap([FromBody] DistrictFilterDto filterDto, CancellationToken cancellationToken)
        {
            return Ok(await bgMapReportService.GetBgMap(filterDto, cancellationToken));
        }
    }
}
