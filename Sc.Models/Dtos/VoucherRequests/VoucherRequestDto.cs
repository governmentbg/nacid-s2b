using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.VoucherRequests;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.VoucherRequests
{
    public class VoucherRequestDto : Entity, IValidate
    {
        public string Code { get; set; }

        public VoucherRequestState State { get; set; }

        public DateTime CreateDate { get; set; }

        public int RequestUserId { get; set; }
        public int RequestCompanyId { get; set; }
        public CompanyDto RequestCompany { get; set; }

        public int SupplierOfferingId { get; set; }
        public SupplierOfferingDto SupplierOffering { get; set; }
        public string DeclineReason { get; set; }

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            if (RequestUserId == 0)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestErrorCode.VoucherRequest_User_Required);
            }

            if (RequestCompanyId == 0)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestErrorCode.VoucherRequest_Company_Required);
            }

            if (SupplierOfferingId == 0)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestErrorCode.VoucherRequest_SupplierOffering_Required);
            }
        }
    }
}
