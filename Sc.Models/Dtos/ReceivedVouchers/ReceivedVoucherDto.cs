using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.ReceivedVouchers.Base;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.ReceivedVouchers
{
    public class ReceivedVoucherDto : BaseReceivedVoucherDto, IValidate
    {
        public ReceivedVoucherFileDto File { get; set; }

        public List<ReceivedVoucherCertificateDto> Certificates { get; set; } = new List<ReceivedVoucherCertificateDto>();
        public int HistoriesCount { get; set; }

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            ContractNumber = ContractNumber?.Trim();
            OfferingClarifications = OfferingClarifications?.Trim();
            SecondOfferingClarifications = SecondOfferingClarifications?.Trim();

            if (CompanyUserId == 0)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_User_Required);
            }

            if (CompanyId == 0)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Company_Required);
            }

            if (ContractDate.Year < 2024)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_ContractDate_Invalid);
            }

            if (string.IsNullOrWhiteSpace(ContractNumber))
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_ContractNumber_Required);
            }

            if (ContractNumber.Length > 100)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_ContractNumber_LengthOverAllowed);
            }

            if (State != ReceivedVoucherState.Draft && State != ReceivedVoucherState.Completed)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_State_Required);
            }

            if ((!SupplierId.HasValue && OfferingId.HasValue) || (SupplierId.HasValue && !OfferingId.HasValue))
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Supplier_Required);
            }

            if ((!SecondSupplierId.HasValue && SecondOfferingId.HasValue) || (SecondSupplierId.HasValue && !SecondOfferingId.HasValue))
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_SecondSupplier_Required);
            }

            if (OfferingId.HasValue && SecondOfferingId.HasValue && OfferingId.Value == SecondOfferingId.Value)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Offering_Duplicate);
            }

            if (!string.IsNullOrWhiteSpace(OfferingClarifications))
            {
                if (OfferingClarifications.Length > 500)
                {
                    domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_OfferingClarifications_LengthOverAllowed);
                }
            }

            if (!string.IsNullOrWhiteSpace(SecondOfferingClarifications))
            {
                if (SecondOfferingClarifications.Length > 500)
                {
                    domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_SecondOfferingClarifications_LengthOverAllowed);
                }
            }

            if (State == ReceivedVoucherState.Completed)
            {
                if (!SupplierId.HasValue)
                {
                    domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Supplier_Required);
                }

                if (!OfferingId.HasValue)
                {
                    domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_SupplierOffering_Required);
                }

                if (File == null)
                {
                    domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_File_Required);
                }
            }
        }
    }
}
