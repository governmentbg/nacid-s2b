using Sc.Models.Dtos.Notifications;

namespace Sc.Models.Dtos.ReceivedVouchers
{
    public class ReceivedVoucherCommunicationDto : BaseCommunicationDto
    {
        public ReceivedVoucherDto Entity { get; set; }
    }
}
