namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum SupplierRepresentativeErrorCode
    {
        SupplierRepresentative_Entity_Exists,

        SupplierRepresentative_Name_Required,
        SupplierRepresentative_Name_LengthOverAllowed,
        SupplierRepresentative_Name_OnlyCyrillicAllowed,

        SupplierRepresentative_NameAlt_LengthOverAllowed,
        SupplierRepresentative_NameAlt_OnlyLatinAllowed,

        SupplierRepresentative_UserName_Required,
        SupplierRepresentative_UserName_Invalid,

        SupplierRepresentative_PhoneNumber_LengthOverAllowed,
        SupplierRepresentative_PhoneNumber_Invalid,

        SupplierRepresentative_Supplier_InvalidSupplierId
    }
}
