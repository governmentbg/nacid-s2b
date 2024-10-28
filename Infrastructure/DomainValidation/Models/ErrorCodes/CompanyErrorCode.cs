namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum CompanyErrorCode
    {
        Company_Name_Required,
        Company_Name_LengthOverAllowed,
        Company_Name_OnlyCyrillicAllowed,

        Company_NameAlt_Required,
        Company_NameAlt_LengthOverAllowed,
        Company_NameAlt_OnlyLatinAllowed,

        Company_District_Required,
        Company_Municipality_Required,
        Company_Settlement_Required,

        Company_Uic_Required,
        Company_Uic_OnlyDigitsAllowed,
        Company_LawForm_Required,
        Company_Address_Required,
        Company_Address_LengthOverAllowed,
        Company_Address_Invalid,
        Company_AddressAlt_Required,
        Company_AddressAlt_LengthOverAllowed,
        Company_AddressAlt_Invalid,
        Company_Email_Required,
        Company_Email_Invalid,
        Company_PhoneNumber_Required,
        Company_PhoneNumber_LengthOverAllowed,
        Company_PhoneNumber_Invalid,
        Company_Entity_Exists,

        CompanyAdditional_StaffCount_Invalid,
        CompanyAdditional_WebPage_Required,
        CompanyAdditional_AnnualTurnover_Invalid,
        CompanyAdditional_Entity_Exists,
        CompanyAdditional_Entity_InvalidId
    }
}
