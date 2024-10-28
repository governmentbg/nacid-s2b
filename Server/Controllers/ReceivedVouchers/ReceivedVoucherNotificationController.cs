using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.DomainValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Services.ReceivedVouchers;

namespace Server.Controllers.ReceivedVouchers
{
    [ApiController]
    [Route("api/notifications/receivedVouchers")]
    public class ReceivedVoucherNotificationController : ControllerBase
    {
        private readonly ReceivedVoucherNotificationService notificationService;
        private readonly DomainValidatorService domainValidatorService;

        public ReceivedVoucherNotificationController(
            ReceivedVoucherNotificationService notificationService,
            DomainValidatorService domainValidatorService)
        {
            this.notificationService = notificationService;
            this.domainValidatorService = domainValidatorService;
        }

        [Authorize, ScClient]
        [HttpDelete("{receivedVoucherId:int}")]
        public async Task<ActionResult> Delete([FromRoute] int receivedVoucherId, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            await notificationService.Delete(receivedVoucherId, cancellationToken);

            return Ok();
        }
    }
}
