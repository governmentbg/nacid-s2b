using Sc.Models.Enums.VoucherRequests;

namespace Sc.Models.Dtos.VoucherRequests
{
    public class ChangedStateDto
    {
        public VoucherRequestState State { get; set; }
        public string Code { get; set; }
    }
}
