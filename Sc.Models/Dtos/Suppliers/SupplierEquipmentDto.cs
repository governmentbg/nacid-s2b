using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Helpers.ValidateProperties;
using Sc.Models.Dtos.Suppliers.Junctions;
using Sc.Models.Entities.Base;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Suppliers
{
    public class SupplierEquipmentDto : Entity, IValidate
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SupplierEquipmentFileDto File { get; set; }

        public List<SupplierOfferingEquipmentDto> SupplierOfferingEquipment { get; set; } = new List<SupplierOfferingEquipmentDto>();

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            Name = Name?.Trim();
            Description = Description?.Trim();

            if (string.IsNullOrWhiteSpace(Name))
            {
                domainValidatorService.ThrowErrorMessage(SupplierEquipmentErrorCode.SupplierEquipment_Name_Required);
            }

            if (Name.Length > 100)
            {
                domainValidatorService.ThrowErrorMessage(SupplierEquipmentErrorCode.SupplierEquipment_Name_LengthOverAllowed);
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                domainValidatorService.ThrowErrorMessage(SupplierEquipmentErrorCode.SupplierEquipment_Description_Required);
            }

            if (Description.Length > 500)
            {
                domainValidatorService.ThrowErrorMessage(SupplierEquipmentErrorCode.SupplierEquipment_Description_LengthOverAllowed);
            }
        }
    }
}
