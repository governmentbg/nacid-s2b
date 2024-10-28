using Infrastructure;
using Infrastructure.Attributes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Reports.ReceivedVouchers;
using Sc.Models.FilterDtos.Reports.ReceivedVouchers;
using Sc.Reports.ReceivedVouchers;

namespace Server.Controllers.Reports
{
    [ApiController]
    [Route("api/report/offeringContract")]
    public class OfferingContractReportController : ControllerBase
    {
        private readonly OfferingContractReportService offeringContractReportService;
        private readonly UserContext userContext;
        private readonly PermissionService permissionService;

        public OfferingContractReportController(
            OfferingContractReportService offeringContractReportService,
            UserContext userContext,
            PermissionService permissionService
           )
        {
            this.offeringContractReportService = offeringContractReportService;
            this.userContext = userContext;
            this.permissionService = permissionService;
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SearchResultDto<OfferingContractReportDto>>> GetReport([FromBody] OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyPermissionException(PermissionConstants.CommonReportsReadPermission);

            return Ok(await offeringContractReportService.GetReport(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost("excel")]
        public async Task<FileStreamResult> ExportExcel([FromBody] OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyPermissionException(PermissionConstants.CommonReportsReadPermission);

            var excelStream = await offeringContractReportService.ExportExcel(filter, cancellationToken);

            return new FileStreamResult(excelStream, "application/vnd.ms-excel");
        }

        [Authorize, ScClient]
        [HttpPost("json")]
        public async Task<FileStreamResult> ExportJson([FromBody] OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyPermissionException(PermissionConstants.CommonReportsReadPermission);

            var jsonMemoryStream = await offeringContractReportService.ExportJson(filter, cancellationToken);

            return new FileStreamResult(jsonMemoryStream, "application/json");
        }

        [Authorize, ScClient]
        [HttpPost("csv")]
        public async Task<FileContentResult> ExportCsv([FromBody] OfferingContractReportFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyPermissionException(PermissionConstants.CommonReportsReadPermission);

            var csvStream = await offeringContractReportService.ExportCsv(filter, cancellationToken);

            return new FileContentResult(csvStream.ToArray(), "application/json");
        }
    }
}
