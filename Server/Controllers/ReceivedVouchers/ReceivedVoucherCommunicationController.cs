using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Services.ReceivedVouchers;
using Sc.Services.ReceivedVouchers.Permissions;

namespace Server.Controllers.ReceivedVouchers
{
    [ApiController]
    [Route("api/receivedVoucherCommunications")]
    public class ReceivedVoucherCommunicationController : ControllerBase
    {
        private readonly ReceivedVoucherCommunicationService communicationService;
        private readonly ReceivedVoucherService receivedVoucherService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ReceivedVoucherPermissionService receivedVoucherPermissionService;
        private readonly UserContext userContext;

        public ReceivedVoucherCommunicationController(
            ReceivedVoucherCommunicationService communicationService,
            ReceivedVoucherService receivedVoucherService,
            DomainValidatorService domainValidatorService,
            ReceivedVoucherPermissionService receivedVoucherPermissionService,
            UserContext userContext
            )
        {
            this.communicationService = communicationService;
            this.receivedVoucherService = receivedVoucherService;
            this.domainValidatorService = domainValidatorService;
            this.receivedVoucherPermissionService = receivedVoucherPermissionService;
            this.userContext = userContext;
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<List<ReceivedVoucherCommunicationDto>>> GetCommunications([FromBody] ReceivedVoucherCommunicationFilterDto filter, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            if (!filter.ReceivedVoucherId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherCommunicationErrorCode.ReceivedVoucherCommunication_ReceivedVoucher_Required);
            }

            if ((!userContext.CompanyId.HasValue || userContext.CompanyId == 0)
                && !userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            var receivedVoucherDto = await receivedVoucherService.GetById(filter.ReceivedVoucherId.Value, IncludeType.NavProperties, cancellationToken);

            await receivedVoucherPermissionService.VerifyReceivedVoucherPermissionException(PermissionConstants.ReceivedVoucherReadPermission, receivedVoucherDto, cancellationToken);

            return Ok(await communicationService.GetCommunications(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost("sendMessage")]
        public async Task<ActionResult<ReceivedVoucherCommunicationDto>> SendMessage([FromBody] ReceivedVoucherCommunicationDto currentCommunicationDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            if ((!userContext.CompanyId.HasValue || userContext.CompanyId == 0)
                && !userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            var receivedVoucherDto = await receivedVoucherService.GetById(currentCommunicationDto.EntityId, IncludeType.NavProperties, cancellationToken);

            await receivedVoucherPermissionService.VerifyReceivedVoucherPermissionException(PermissionConstants.ReceivedVoucherWritePermission, receivedVoucherDto, cancellationToken);

            return Ok(await communicationService.SendMessage(receivedVoucherDto.Id, currentCommunicationDto, cancellationToken));
        }
    }
}
