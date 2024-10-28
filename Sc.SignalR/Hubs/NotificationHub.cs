using Microsoft.AspNetCore.SignalR;
using Sc.Models.Dtos.Notifications;
using Sc.SignalR.Services;

namespace Sc.SignalR.Hubs
{
    public interface INotificationHub
    {
        Task SendNotification(NotificationDto notification);
    }

    public class NotificationHub : Hub<INotificationHub>
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"].ToString();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                ConnectionUserMapping.Add(int.Parse(userId.Trim()), Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.GetHttpContext().Request.Query["userId"].ToString();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                ConnectionUserMapping.Remove(int.Parse(userId.Trim()), Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
