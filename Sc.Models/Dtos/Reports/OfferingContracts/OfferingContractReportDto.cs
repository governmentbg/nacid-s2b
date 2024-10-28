using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Dtos.Reports.ReceivedVouchers
{
    public class OfferingContractReportDto
    {
        public int SupplierId { get; set; }
        public SupplierType SupplierType { get; set; }
        public string SupplierName { get; set; }

        public string InstitutionRootName { get; set; }
        public int? InstitutionRootId { get; set; }
        public int? InstitutionId { get; set; }
        public int? ComplexId { get; set; }

        public int OfferingId { get; set; }
        public string OfferingCode { get; set; }
        public string OfferingName { get; set; }

        public int OfferingsCount { get; set; }
    }
}
