namespace Infrastructure.DomainValidation.Models.ErrorCodes
{
    public enum NomenclatureErrorCode
    {
        Nomenclature_Name_Required,
        Nomenclature_Name_LengthOverAllowed,
        Nomenclature_Name_OnlyCyrillicAllowed,

        Nomenclature_NameAlt_Required,
        Nomenclature_NameAlt_LengthOverAllowed,
        Nomenclature_NameAlt_OnlyLatinAllowed,

        Nomenclature_Entity_HasReference
    }
}
