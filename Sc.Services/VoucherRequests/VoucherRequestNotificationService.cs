using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.Notifications;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.VoucherRequests;
using Sc.Repositories.VoucherRequests;
using Sc.SignalR.Hubs;
using Sc.SignalR.Services;

namespace Sc.Services.VoucherRequests
{
    public class VoucherRequestNotificationService
    {
        private readonly IVoucherRequestRepository voucherRequestRepository;
        private readonly IVoucherRequestNotificationRepository notificationRepository;
        private readonly UserContext userContext;
        private IHubContext<NotificationHub, INotificationHub> notificationHub;


        public VoucherRequestNotificationService(
            IVoucherRequestRepository voucherRequestRepository,
            IVoucherRequestNotificationRepository notificationRepository,
            IHubContext<NotificationHub, INotificationHub> notificationHub,
            UserContext userContext
            )
        {
            this.voucherRequestRepository = voucherRequestRepository;
            this.notificationRepository = notificationRepository;
            this.notificationHub = notificationHub;
            this.userContext = userContext;
        }

        public async Task<List<NotificationDto>> GetNotifications(CancellationToken cancellationToken)
        {
            var voucherRequestNotifications = await notificationRepository
                .GetListByProperties(e => e.ToUserId == userContext.UserId, cancellationToken, e => e.Include(s => s.Entity.SupplierOffering));

            var groupedNotifications = voucherRequestNotifications
                .GroupBy(e => new { e.FromUserId, e.EntityId })
                .Select(s => new NotificationDto
                {
                    CreateDate = s.Last().CreateDate,
                    FromFullname = s.Last().FromFullname,
                    FromUserId = s.Key.FromUserId,
                    EntityId = s.Key.EntityId,
                    FromUsername = s.Last().FromUsername,
                    FromUserOrganization = s.Last().FromUserOrganization,
                    CompanyId = s.Last().Entity.RequestCompanyId,
                    VrState = null,
                    RvState = null,
                    SupplierId = s.Last().Entity.SupplierOffering.SupplierId,
                    OfferingId = s.Last().Entity.SupplierOfferingId,
                    Text = $"Имате {s.Count()} непрочетени съобщения, свързани с услугата \"{s.Last().Entity.SupplierOffering.Name}\".",
                    ToUserId = s.Last().ToUserId,
                    Type = NotificationType.Message,
                    EntityType = NotificationEntityType.VoucherRequest
                }).ToList();

            return groupedNotifications;
        }

        public async Task CreateNotifications(int voucherRequestId, DateTime createDate, NotificationType notificationType, VoucherRequestState? voucherRequestState, string code, string text, CancellationToken cancellationToken)
        {
            var voucherRequest = await voucherRequestRepository.GetById(voucherRequestId, cancellationToken, e => e
                .Include(s => s.RequestCompany)
                .Include(s => s.SupplierOffering.Supplier.Representative)
                .Include(s => s.SupplierOffering.SupplierOfferingTeams)
                    .ThenInclude(m => m.SupplierTeam));

            var userIdsToRecieveNotification = new List<int>();

            if (userContext.UserId != voucherRequest.RequestUserId)
            {
                userIdsToRecieveNotification.Add(voucherRequest.RequestUserId);
            }

            if (userContext.UserId != voucherRequest.SupplierOffering.Supplier.Representative.UserId)
            {
                userIdsToRecieveNotification.Add(voucherRequest.SupplierOffering.Supplier.Representative.UserId);
            }

            var peopleInTeamIds = voucherRequest
                .SupplierOffering.SupplierOfferingTeams
                .Where(e => e.SupplierTeam.UserId != userContext.UserId)
                .Select(e => e.SupplierTeam.UserId)
                .ToList();

            if (peopleInTeamIds.Any())
            {
                userIdsToRecieveNotification.AddRange(peopleInTeamIds);
            }

            userIdsToRecieveNotification = userIdsToRecieveNotification.Distinct().ToList();

            var organizationName = userContext.CompanyId.HasValue ? voucherRequest.RequestCompany.Name : userContext.OrganizationalUnits.FirstOrDefault(e => e.SupplierId == voucherRequest.SupplierOffering.SupplierId)?.ShortName;

            var voucherRequestNotificationsForCreate = new List<VoucherRequestNotification>();

            foreach (var userIdToRecieveNotification in userIdsToRecieveNotification)
            {
                voucherRequestNotificationsForCreate.Add(new VoucherRequestNotification
                {
                    ChangedToState = voucherRequestState,
                    CreateDate = createDate,
                    Code = code,
                    FromFullname = userContext.FullName,
                    FromUserId = userContext.UserId.Value,
                    FromUsername = userContext.UserName,
                    FromUserOrganization = organizationName,
                    Text = text,
                    Type = notificationType,
                    EntityId = voucherRequestId,
                    ToUserId = userIdToRecieveNotification
                });

                var notification = new NotificationDto
                {
                    EntityId = voucherRequest.Id,
                    EntityType = NotificationEntityType.VoucherRequest,
                    CompanyId = voucherRequest.RequestCompanyId,
                    OfferingId = voucherRequest.SupplierOfferingId,
                    SupplierId = voucherRequest.SupplierOffering.SupplierId,
                    CreateDate = createDate,
                    FromFullname = userContext.FullName,
                    FromUserId = userContext.UserId.Value,
                    FromUsername = userContext.UserName,
                    FromUserOrganization = organizationName,
                    Type = notificationType,
                    VrState = voucherRequestState.HasValue ? voucherRequest.State : null,
                    Code = code,
                    Text = text,
                    ToUserId = userIdToRecieveNotification
                };

                await SendNotification(notification);
            }

            await notificationRepository.CreateRange(voucherRequestNotificationsForCreate);
        }

        public async Task Delete(int voucherRequestId, CancellationToken cancellationToken)
        {
            var voucherRequestsNotificationsForDelete = await notificationRepository
                .GetListByProperties(e => e.EntityId == voucherRequestId && e.ToUserId == userContext.UserId, cancellationToken);

            foreach (var voucherRequestNotificationsForDelete in voucherRequestsNotificationsForDelete)
            {
                try
                {
                    await notificationRepository.Delete(voucherRequestNotificationsForDelete);
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task SendNotification(NotificationDto notification)
        {
            foreach (var connectionId in ConnectionUserMapping.GetConnections(notification.ToUserId))
            {
                await notificationHub
                .Clients
                .Client(connectionId)
                .SendNotification(notification);
            }
        }
    }
}
