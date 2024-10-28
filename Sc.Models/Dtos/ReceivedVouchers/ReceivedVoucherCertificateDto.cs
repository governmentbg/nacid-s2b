using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.ReceivedVouchers
{
    public class ReceivedVoucherCertificateDto : Entity
    {
        public int ReceivedVoucherId { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserFullname { get; set; }

        public ReceivedVoucherCertificateFileDto File { get; set; }

        public int SupplierId { get; set; }
        public SupplierDto Supplier { get; set; }

        public int OfferingId { get; set; }
        public SupplierOfferingDto Offering { get; set; }
    }
}
