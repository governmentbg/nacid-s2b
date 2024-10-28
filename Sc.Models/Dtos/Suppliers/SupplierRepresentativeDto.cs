using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Helpers.ValidateProperties;
using Sc.Models.Entities.Base;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Suppliers
{
    public class SupplierRepresentativeDto : Entity, IValidate
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }
        public string NameAlt { get; set; }

        public string PhoneNumber { get; set; }

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            UserName = UserName?.Trim();
            Name = Name?.Trim();
            NameAlt = NameAlt?.Trim();
            PhoneNumber = PhoneNumber?.Trim();

            if (string.IsNullOrWhiteSpace(Name))
            {
                domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_Name_Required);
            }

            if (Name.Length > 100)
            {
                domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_Name_LengthOverAllowed);
            }

            if (!ValidatePropertiesHelper.IsValidCyrillicName(Name))
            {
                domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_Name_OnlyCyrillicAllowed);
            }

            if (!string.IsNullOrWhiteSpace(NameAlt))
            {
                if (NameAlt.Length > 100)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_NameAlt_LengthOverAllowed);
                }

                if (!ValidatePropertiesHelper.IsValidLatinName(NameAlt))
                {
                    domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_NameAlt_OnlyLatinAllowed);
                }
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_UserName_Required);
            }

            if (!ValidatePropertiesHelper.IsValidEmail(UserName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_UserName_Invalid);
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                if (PhoneNumber.Length > 18)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_PhoneNumber_LengthOverAllowed);
                }

                if (!ValidatePropertiesHelper.IsValidPhoneNumber(PhoneNumber))
                {
                    domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_PhoneNumber_Invalid);
                }
            }
        }
    }
}
