using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Helpers.ValidateProperties;
using Sc.Models.Dtos.Nomenclatures;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.Companies;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Companies
{
    public class CompanyDto : Entity, IValidate
    {
        public string Uic { get; set; }

        public CompanyType Type { get; set; }

        public int LawFormId { get; set; }
        public LawFormDto LawForm { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }

        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

        public int SettlementId { get; set; }
        public SettlementDto Settlement { get; set; }
        public int MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
        public int DistrictId { get; set; }
        public DistrictDto District { get; set; }

        public string Address { get; set; }
        public string AddressAlt { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public CompanyRepresentativeDto Representative { get; set; }
        public CompanyAdditionalDto CompanyAdditional { get; set; }

        public bool IsActive { get; set; }

        public bool IsRegistryAgency { get; set; }


        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            Uic = Uic?.Trim();
            Name = Name?.Trim();
            NameAlt = NameAlt?.Trim();
            // There will be no short name, so short name is going to be name
            ShortName = Name?.Trim();
            ShortNameAlt = NameAlt?.Trim();
            Address = Address?.Trim();
            AddressAlt = AddressAlt?.Trim();
            Email = Email?.Trim();
            PhoneNumber = PhoneNumber?.Trim();

            if (string.IsNullOrWhiteSpace(Uic))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Uic_Required);
            }

            if (!ValidatePropertiesHelper.IsDigitsOnly(Uic))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Uic_OnlyDigitsAllowed);
            }

            if (LawFormId == 0)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_LawForm_Required);
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Name_Required);
            }

            if (Name.Length > 100)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Name_LengthOverAllowed);
            }

            if (DistrictId == 0)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_District_Required);
            }

            if (MunicipalityId == 0)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Municipality_Required);
            }

            if (SettlementId == 0)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Settlement_Required);
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Address_Required);
            }

            if (Address.Length > 250)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Address_LengthOverAllowed);
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Email_Required);
            }

            if (!ValidatePropertiesHelper.IsValidEmail(Email))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_Email_Invalid);
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_PhoneNumber_Required);
            }

            if (PhoneNumber.Length > 18)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_PhoneNumber_LengthOverAllowed);
            }

            if (!ValidatePropertiesHelper.IsValidPhoneNumber(PhoneNumber))
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.Company_PhoneNumber_Invalid);
            }
        }
    }
}
