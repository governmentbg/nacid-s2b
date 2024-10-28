using Sc.Models.Enums.VoucherRequests;

namespace Sc.Models.Dtos.VoucherRequests
{
    public class VoucherRequestStateDto
    {
        public int RequestCompanyId { get; set; }
        public int SupplierOfferingId { get; set; }
        public VoucherRequestState State { get; set; }
    }
}
