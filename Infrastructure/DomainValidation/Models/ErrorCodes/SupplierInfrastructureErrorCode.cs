namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum SupplierEquipmentErrorCode
    {
        SupplierEquipment_Name_Required,
        SupplierEquipment_Name_LengthOverAllowed,
        SupplierEquipment_Name_OnlyCyrillicAllowed,
        SupplierEquipment_NameAlt_Required,
        SupplierEquipment_NameAlt_LengthOverAllowed,
        SupplierEquipment_NameAlt_OnlyLatinAllowed,
        SupplierEquipment_Description_Required,
        SupplierEquipment_Description_LengthOverAllowed,
        SupplierEquipment_Description_OnlyCyrillicAllowed,
        SupplierEquipment_DescriptionAlt_Required,
        SupplierEquipment_DescriptionAlt_LengthOverAllowed,
        SupplierEquipment_DescriptionAlt_OnlyLatinAllowed,
        SupplierEquipment_SmartSpecialization_Required,

        SupplierEquipment_Supplier_InvalidSupplierId
    }
}
