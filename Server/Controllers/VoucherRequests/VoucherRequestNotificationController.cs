using Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Services.VoucherRequests;

namespace Server.Controllers.VoucherRequests
{
    [ApiController]
    [Route("api/notifications/voucherRequests")]
    public class VoucherRequestNotificationController : ControllerBase
    {
        private readonly VoucherRequestNotificationService notificationService;

        public VoucherRequestNotificationController(
            VoucherRequestNotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [Authorize, ScClient]
        [HttpDelete("{voucherRequestId:int}")]
        public async Task<ActionResult> Delete([FromRoute] int voucherRequestId, CancellationToken cancellationToken)
        {
            await notificationService.Delete(voucherRequestId, cancellationToken);

            return Ok();
        }
    }
}
