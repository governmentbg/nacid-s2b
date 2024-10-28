namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum SupplierTeamErrorCode
    {
        SupplierTeam_Supplier_InvalidSupplierId,

        SupplierTeam_UserName_DoesNotMatch,

        SupplierTeam_Position_LengthOverAllowed,
        SupplierTeam_AcademicRankDegree_LengthOverAllowed,

        SupplierTeam_FirstName_Required,
        SupplierTeam_FirstName_LengthOverAllowed,
        SupplierTeam_FirstName_OnlyCyrillicAllowed,

        SupplierTeam_MiddleName_LengthOverAllowed,
        SupplierTeam_MiddleName_OnlyCyrillicAllowed,

        SupplierTeam_LastName_Required,
        SupplierTeam_LastName_LengthOverAllowed,
        SupplierTeam_LastName_OnlyCyrillicAllowed,

        SupplierTeam_Name_Required,
        SupplierTeam_Name_LengthOverAllowed,
        SupplierTeam_Name_OnlyCyrillicAllowed,

        SupplierTeam_Email_Required,
        SupplierTeam_Email_Invalid,

        SupplierTeam_PhoneNumber_LengthOverAllowed,
        SupplierTeam_PhoneNumber_Invalid,

        SupplierTeam_SpecificServicesOffered_Required,
        SupplierTeam_SpecificServicesOfferedAlt_Required,

        SupplierTeam_Entity_Exists,
        SupplierTeam_Entity_CannotChangeSupplierId,

        SupplierTeam_Representative_CannotBeInTeam
    }
}
