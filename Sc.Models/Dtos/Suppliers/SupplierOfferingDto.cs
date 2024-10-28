using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Dtos.Suppliers.Junctions;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.Suppliers;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Suppliers
{
    public class SupplierOfferingDto : Entity, IValidate
    {
        public int SupplierId { get; set; }
        public SupplierDto Supplier { get; set; }

        public string Code { get; set; }

        public OfferingType OfferingType { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public int SettlementId { get; set; }
        public SettlementDto Settlement { get; set; }
        public int MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
        public int DistrictId { get; set; }
        public DistrictDto District { get; set; }
        public string Address { get; set; }
        public string WebPageUrl { get; set; }

        public bool IsActive { get; set; }

        public List<SupplierOfferingFileDto> Files { get; set; } = new List<SupplierOfferingFileDto>();

        public List<SupplierOfferingSmartSpecializationDto> SmartSpecializations { get; set; } = new List<SupplierOfferingSmartSpecializationDto>();

        public List<SupplierOfferingTeamDto> SupplierOfferingTeams { get; set; } = new List<SupplierOfferingTeamDto>();
        public List<SupplierOfferingEquipmentDto> SupplierOfferingEquipment { get; set; } = new List<SupplierOfferingEquipmentDto>();

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            Name = Name?.Trim();
            ShortDescription = ShortDescription?.Trim();
            Description = Description?.Trim();
            Address = Address?.Trim();

            if (string.IsNullOrWhiteSpace(Name))
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Name_Required);
            }

            if (Name.Length > 200)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Name_LengthOverAllowed);
            }

            if (string.IsNullOrWhiteSpace(ShortDescription))
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_ShortDescription_Required);
            }

            if (ShortDescription.Length > 250)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_ShortDescription_LengthOverAllowed);
            }

            if (!string.IsNullOrWhiteSpace(Address))
            {
                if (Address.Length > 250)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Address_LengthOverAllowed);
                }
            }

            if (!string.IsNullOrWhiteSpace(WebPageUrl))
            {
                if (WebPageUrl.Length > 100)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_WebPageUrl_LengthOverAllowed);
                }
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                if (Description.Length > 500)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Description_LengthOverAllowed);
                }
            }

            if (!SmartSpecializations.Any() || SmartSpecializations.Count < 1)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_SmartSpecializations_Required);
            }

            if (SmartSpecializations.Count(e => e.Type == SupplierOfferingSmartSpecializationType.Primary) != 1)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_SmartSpecializations_MustHaveOnePrimary);
            }

            if (Files.Count > 3)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Files_CountNotAllowed);
            }
          
        }
    }
}
