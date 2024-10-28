namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum ReceivedVoucherErrorCode
    {
        ReceivedVoucher_User_Required,
        ReceivedVoucher_Company_Required,
        ReceivedVoucher_ContractDate_Invalid,
        ReceivedVoucher_ContractNumber_Required,
        ReceivedVoucher_ContractNumber_LengthOverAllowed,
        ReceivedVoucher_State_Required,
        ReceivedVoucher_Supplier_Required,
        ReceivedVoucher_SecondSupplier_Required,
        ReceivedVoucher_Offering_Duplicate,
        ReceivedVoucher_OfferingClarifications_LengthOverAllowed,
        ReceivedVoucher_SecondOfferingClarifications_LengthOverAllowed,
        ReceivedVoucher_SupplierOffering_Required,
        ReceivedVoucher_File_Required,
        ReceivedVoucher_Entity_Exists,
        ReceivedVoucher_Certificate_FileRequired,
        ReceivedVoucher_Certificate_AllAreGenerated,
        ReceivedVoucher_Certificate_AlreadyGenerated,
        ReceivedVoucher_Certificate_InvalidSupplier,
        ReceivedVoucher_Certificate_InvalidOffering
    }
}
