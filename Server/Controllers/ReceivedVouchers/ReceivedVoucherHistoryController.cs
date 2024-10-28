using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.DomainValidation;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Services.ReceivedVouchers;
using Sc.Services.ReceivedVouchers.Permissions;

namespace Server.Controllers.ReceivedVouchers
{
    [ApiController]
    [Route("api/receivedVouchers/{receivedVoucherId:int}/history")]
    public class ReceivedVoucherHistoryController : ControllerBase
    {
        private readonly ReceivedVoucherHistoryService receivedVoucherHistoryService;
        private readonly ReceivedVoucherService receivedVoucherService;
        private readonly ReceivedVoucherPermissionService receivedVoucherPermissionService;
        private readonly DomainValidatorService domainValidatorService;

        public ReceivedVoucherHistoryController(
            ReceivedVoucherHistoryService receivedVoucherHistoryService,
            ReceivedVoucherService receivedVoucherService,
            ReceivedVoucherPermissionService receivedVoucherPermissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.receivedVoucherHistoryService = receivedVoucherHistoryService;
            this.receivedVoucherService = receivedVoucherService;
            this.receivedVoucherPermissionService = receivedVoucherPermissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [Authorize, ScClient]
        [HttpPost("search")]
        public async Task<ActionResult<SearchResultDto<ReceivedVoucherHistoryDto>>> GetSearchResultDto([FromBody] ReceivedVoucherHistoryFilterDto filter, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var receivedVoucherDto = await receivedVoucherService.GetById(filter.ReceivedVoucherId, IncludeType.All, cancellationToken);

            await receivedVoucherPermissionService.VerifyReceivedVoucherPermissionException(PermissionConstants.ReceivedVoucherReadPermission, receivedVoucherDto, cancellationToken);

            return Ok(await receivedVoucherHistoryService.GetSearchResultDto<ReceivedVoucherHistoryDto>(filter, cancellationToken));
        }
    }
}
