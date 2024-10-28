using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Companies;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.ReceivedVouchers;

namespace Sc.Models.Entities.ReceivedVouchers.Base
{
    public abstract class BaseReceivedVoucher : EntityVersion
    {
        public DateTime ContractDate { get; set; }
        public string ContractNumber { get; set; }

        public ReceivedVoucherState State { get; set; }

        public int CompanyUserId { get; set; }
        public int CompanyId { get; set; }
        [Skip]
        public Company Company { get; set; }

        public int? SupplierId { get; set; }
        [Skip]
        public Supplier Supplier { get; set; }
        public int? OfferingId { get; set; }
        [Skip]
        public SupplierOffering Offering { get; set; }
        public bool OfferingAdditionalPayment { get; set; }
        public string ReceivedOffering { get; set; }
        public string OfferingClarifications { get; set; }

        public int? SecondSupplierId { get; set; }
        [Skip]
        public Supplier SecondSupplier { get; set; }
        public int? SecondOfferingId { get; set; }
        [Skip]
        public SupplierOffering SecondOffering { get; set; }
        public bool SecondOfferingAdditionalPayment { get; set; }
        public string SecondReceivedOffering { get; set; }
        public string SecondOfferingClarifications { get; set; }
    }
}
