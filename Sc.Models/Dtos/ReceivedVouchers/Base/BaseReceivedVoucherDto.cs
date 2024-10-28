using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.ReceivedVouchers;

namespace Sc.Models.Dtos.ReceivedVouchers.Base
{
    public abstract class BaseReceivedVoucherDto : Entity
    {
        public DateTime ContractDate { get; set; }
        public string ContractNumber { get; set; }

        public ReceivedVoucherState State { get; set; }

        public int CompanyUserId { get; set; }
        public int CompanyId { get; set; }
        public CompanyDto Company { get; set; }

        public int? SupplierId { get; set; }
        public SupplierDto Supplier { get; set; }
        public int? OfferingId { get; set; }
        public SupplierOfferingDto Offering { get; set; }
        public bool OfferingAdditionalPayment { get; set; }
        public string ReceivedOffering { get; set; }
        public string OfferingClarifications { get; set; }

        public int? SecondSupplierId { get; set; }
        public SupplierDto SecondSupplier { get; set; }
        public int? SecondOfferingId { get; set; }
        public SupplierOfferingDto SecondOffering { get; set; }
        public bool SecondOfferingAdditionalPayment { get; set; }
        public string SecondReceivedOffering { get; set; }
        public string SecondOfferingClarifications { get; set; }
    }
}
