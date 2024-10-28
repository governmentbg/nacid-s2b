using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Helpers.ValidateProperties;
using Sc.Models.Dtos.Suppliers.Junctions;
using Sc.Models.Entities.Base;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Suppliers
{
    public class SupplierTeamDto : Entity, IValidate
    {
        public int SupplierId { get; set; }
        public SupplierDto Supplier { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserNameAgain { get; set; }

        public string AcademicRankDegree { get; set; }
        public string Position { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int? RasLotId { get; set; }
        public int? RasLotNumber { get; set; }
        public string RasPortalUrl { get; set; }

        public bool IsActive { get; set; }

        public List<SupplierOfferingTeamDto> SupplierOfferingTeams { get; set; } = new List<SupplierOfferingTeamDto>();

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            UserName = UserName?.Trim();
            UserNameAgain = UserNameAgain?.Trim();
            Position = Position?.Trim();
            AcademicRankDegree = AcademicRankDegree?.Trim();
            FirstName = FirstName?.Trim();
            MiddleName = MiddleName?.Trim();
            LastName = LastName?.Trim();
            Name = $"{FirstName}{(!string.IsNullOrWhiteSpace(MiddleName) ? $" {MiddleName?.Trim()}" : string.Empty)} {LastName.Trim()}";
            PhoneNumber = PhoneNumber?.Trim();
            RasPortalUrl = RasPortalUrl?.Trim();

            if (!string.IsNullOrWhiteSpace(Position))
            {
                if (Position.Length > 50)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Position_LengthOverAllowed);
                }
            }

            if (!string.IsNullOrWhiteSpace(AcademicRankDegree))
            {
                if (AcademicRankDegree.Length > 50)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_AcademicRankDegree_LengthOverAllowed);
                }
            }

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_FirstName_Required);
            }

            if (FirstName.Length > 50)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_FirstName_LengthOverAllowed);
            }

            if (!ValidatePropertiesHelper.IsValidCyrillicName(FirstName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_FirstName_OnlyCyrillicAllowed);
            }

            if (!string.IsNullOrWhiteSpace(MiddleName))
            {
                if (MiddleName.Length > 50)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_MiddleName_LengthOverAllowed);
                }

                if (!ValidatePropertiesHelper.IsValidCyrillicName(MiddleName))
                {
                    domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_MiddleName_OnlyCyrillicAllowed);
                }
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_LastName_Required);
            }

            if (LastName.Length > 50)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_LastName_LengthOverAllowed);
            }

            if (!ValidatePropertiesHelper.IsValidCyrillicName(LastName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_LastName_OnlyCyrillicAllowed);
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Name_Required);
            }

            if (Name.Length > 250)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Name_LengthOverAllowed);
            }

            if (!ValidatePropertiesHelper.IsValidCyrillicName(Name))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Name_OnlyCyrillicAllowed);
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Email_Required);
            }

            if (!ValidatePropertiesHelper.IsValidEmail(UserName))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Email_Invalid);
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                if (PhoneNumber.Length > 18)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_PhoneNumber_LengthOverAllowed);
                }

                if (!ValidatePropertiesHelper.IsValidPhoneNumber(PhoneNumber))
                {
                    domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_PhoneNumber_Invalid);
                }
            }
        }
    }
}
