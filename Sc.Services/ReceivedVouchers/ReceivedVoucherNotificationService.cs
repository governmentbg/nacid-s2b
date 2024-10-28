using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.Notifications;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Repositories.ReceivedVouchers;
using Sc.SignalR.Hubs;
using Sc.SignalR.Services;

namespace Sc.Services.ReceivedVouchers
{
    public class ReceivedVoucherNotificationService
    {
        private readonly IReceivedVoucherRepository receivedVoucherRepository;
        private readonly IReceivedVoucherNotificationRepository notificationRepository;
        private readonly UserContext userContext;
        private IHubContext<NotificationHub, INotificationHub> notificationHub;

        public ReceivedVoucherNotificationService(
            IReceivedVoucherRepository receivedVoucherRepository,
            IReceivedVoucherNotificationRepository notificationRepository,
            IHubContext<NotificationHub, INotificationHub> notificationHub,
            UserContext userContext
            )
        {
            this.receivedVoucherRepository = receivedVoucherRepository;
            this.notificationRepository = notificationRepository;
            this.notificationHub = notificationHub;
            this.userContext = userContext;
        }

        public async Task<List<NotificationDto>> GetNotifications(CancellationToken cancellationToken)
        {
            var receivedVoucherNotifications = await notificationRepository
                .GetListByProperties(e => e.ToUserId == userContext.UserId, cancellationToken, e => e.Include(s => s.Entity));

            var groupedNotifications = receivedVoucherNotifications
                .GroupBy(e => new { e.FromUserId, e.EntityId })
                .Select(s => new NotificationDto
                {
                    CreateDate = s.Last().CreateDate,
                    FromFullname = s.Last().FromFullname,
                    FromUserId = s.Key.FromUserId,
                    EntityId = s.Key.EntityId,
                    FromUsername = s.Last().FromUsername,
                    FromUserOrganization = s.Last().FromUserOrganization,
                    VrState = null,
                    RvState = null,
                    Text = $"Имате {s.Count()} непрочетени съобщения, свързани с получения ваучер с номер на договор \"{s.Last().Entity.ContractNumber}\".",
                    ToUserId = s.Last().ToUserId,
                    Type = NotificationType.Message,
                    EntityType = NotificationEntityType.ReceivedVoucher
                }).ToList();

            return groupedNotifications;
        }

        public async Task CreateNotifications(int receivedVoucherId, DateTime createDate, NotificationType notificationType, ReceivedVoucherState? receivedVoucherState, string text, CancellationToken cancellationToken)
        {
            var receivedVoucher = await receivedVoucherRepository.GetById(receivedVoucherId, cancellationToken, e => e
                .Include(s => s.Company)
                .Include(s => s.Supplier.Representative)
                .Include(s => s.Offering.SupplierOfferingTeams)
                    .ThenInclude(m => m.SupplierTeam)
                .Include(s => s.SecondSupplier.Representative)
                .Include(s => s.SecondOffering.SupplierOfferingTeams)
                    .ThenInclude(m => m.SupplierTeam));

            var userIdsToRecieveNotification = new List<int>();

            if (userContext.UserId != receivedVoucher.CompanyUserId)
            {
                userIdsToRecieveNotification.Add(receivedVoucher.CompanyUserId);
            }

            if (receivedVoucher.Supplier != null && userContext.UserId != receivedVoucher.Supplier.Representative.UserId)
            {
                userIdsToRecieveNotification.Add(receivedVoucher.Supplier.Representative.UserId);
            }

            if (receivedVoucher.SecondSupplier != null && userContext.UserId != receivedVoucher.SecondSupplier.Representative.UserId)
            {
                userIdsToRecieveNotification.Add(receivedVoucher.SecondSupplier.Representative.UserId);
            }

            if (receivedVoucher.Offering != null && receivedVoucher.Offering.SupplierOfferingTeams.Any())
            {
                var peopleInTeamIds = receivedVoucher
                .Offering.SupplierOfferingTeams
                .Where(e => e.SupplierTeam.UserId != userContext.UserId)
                .Select(e => e.SupplierTeam.UserId)
                .ToList();

                if (peopleInTeamIds.Any())
                {
                    userIdsToRecieveNotification.AddRange(peopleInTeamIds);
                }
            }

            if (receivedVoucher.SecondOffering != null && receivedVoucher.SecondOffering.SupplierOfferingTeams.Any())
            {
                var peopleInSecondTeamIds = receivedVoucher
                .SecondOffering.SupplierOfferingTeams
                .Where(e => e.SupplierTeam.UserId != userContext.UserId)
                .Select(e => e.SupplierTeam.UserId)
                .ToList();

                if (peopleInSecondTeamIds.Any())
                {
                    userIdsToRecieveNotification.AddRange(peopleInSecondTeamIds);
                }
            }

            userIdsToRecieveNotification = userIdsToRecieveNotification.Distinct().ToList();

            var organizationName = userContext.CompanyId.HasValue ? receivedVoucher.Company.Name : userContext.OrganizationalUnits.FirstOrDefault(e => e.SupplierId == receivedVoucher.SupplierId || e.SupplierId == receivedVoucher.SecondSupplierId)?.ShortName;

            var receivedVoucherNotificationsForCreate = new List<ReceivedVoucherNotification>();

            foreach (var userIdToRecieveNotification in userIdsToRecieveNotification)
            {
                receivedVoucherNotificationsForCreate.Add(new ReceivedVoucherNotification
                {
                    ChangedToState = receivedVoucherState,
                    CreateDate = createDate,
                    FromFullname = userContext.FullName,
                    FromUserId = userContext.UserId.Value,
                    FromUsername = userContext.UserName,
                    FromUserOrganization = organizationName,
                    Text = text,
                    Type = notificationType,
                    EntityId = receivedVoucherId,
                    ToUserId = userIdToRecieveNotification
                });

                var notification = new NotificationDto
                {
                    EntityId = receivedVoucher.Id,
                    EntityType = NotificationEntityType.ReceivedVoucher,
                    CompanyId = receivedVoucher.CompanyId,
                    CreateDate = createDate,
                    FromFullname = userContext.FullName,
                    FromUserId = userContext.UserId.Value,
                    FromUsername = userContext.UserName,
                    FromUserOrganization = organizationName,
                    Type = notificationType,
                    RvState = receivedVoucherState.HasValue ? receivedVoucher.State : null,
                    Text = text,
                    ToUserId = userIdToRecieveNotification
                };

                await SendNotification(notification);
            }

            await notificationRepository.CreateRange(receivedVoucherNotificationsForCreate);
        }

        public async Task Delete(int receivedVoucherId, CancellationToken cancellationToken)
        {
            var receivedVoucherNotificationsForDelete = await notificationRepository
                .GetListByProperties(e => e.EntityId == receivedVoucherId && e.ToUserId == userContext.UserId, cancellationToken);

            foreach (var receivedVoucherNotificationForDelete in receivedVoucherNotificationsForDelete)
            {
                try
                {
                    await notificationRepository.Delete(receivedVoucherNotificationForDelete);
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
