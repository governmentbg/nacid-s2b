namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum SupplierOfferingErrorCode
    {
        SupplierOffering_Name_Required,
        SupplierOffering_Name_LengthOverAllowed,
        SupplierOffering_Name_OnlyCyrillicAllowed,

        SupplierOffering_ShortDescription_Required,
        SupplierOffering_ShortDescription_LengthOverAllowed,
        SupplierOffering_ShortDescription_OnlyCyrillicAllowed,

        SupplierOffering_Description_OnlyCyrillicAllowed,

        SupplierOffering_Address_LengthOverAllowed,
        SupplierOffering_Address_OnlyCyrillicAllowed,

        SupplierOffering_Supplier_InvalidSupplierId,

        SupplierOffering_WebPageUrl_LengthOverAllowed,

        SupplierOffering_Description_LengthOverAllowed,

        SupplierOffering_SmartSpecializations_Required,
        SupplierOffering_SmartSpecializations_Exists,
        SupplierOffering_SmartSpecializations_CannotChangeSupplierOfferingId,
        SupplierOffering_SmartSpecializations_MustHaveOnePrimary,

        SupplierOffering_Delete_ReferenceExisting,

        SupplierOffering_Files_CountNotAllowed,

        SupplierOffering_Entity_Inactive
    }
}
