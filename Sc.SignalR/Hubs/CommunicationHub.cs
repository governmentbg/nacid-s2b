using Microsoft.AspNetCore.SignalR;
using Sc.Models.Dtos.Notifications;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Dtos.VoucherRequests;

namespace Sc.SignalR.Hubs
{
    public interface ICommunicationHub<T>
        where T : BaseCommunicationDto
    {
        Task SendText(T communication);
    }

    public class VrCommunicationHub : Hub<ICommunicationHub<VoucherRequestCommunicationDto>>
    {
        public async Task JoinGroup(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveGroup(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }

    public class RvCommunicationHub : Hub<ICommunicationHub<ReceivedVoucherCommunicationDto>>
    {
        public async Task JoinGroup(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveGroup(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
