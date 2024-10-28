using Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Notifications;
using Sc.Services.ReceivedVouchers;
using Sc.Services.VoucherRequests;

namespace Server.Controllers.Notifications
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly VoucherRequestNotificationService voucherRequestNotificationService;
        private readonly ReceivedVoucherNotificationService receivedVoucherNotificationService;

        public NotificationController(
            VoucherRequestNotificationService voucherRequestNotificationService,
            ReceivedVoucherNotificationService receivedVoucherNotificationService
            )
        {
            this.voucherRequestNotificationService = voucherRequestNotificationService;
            this.receivedVoucherNotificationService = receivedVoucherNotificationService;
        }

        [Authorize, ScClient]
        [HttpGet]
        public async Task<ActionResult<List<NotificationDto>>> GetNotifications(CancellationToken cancellationToken)
        {
            var vrNotifications = await voucherRequestNotificationService.GetNotifications(cancellationToken);
            var rVNotifications = await receivedVoucherNotificationService.GetNotifications(cancellationToken);

            return Ok(vrNotifications.Concat(rVNotifications).ToList());
        }
    }
}
