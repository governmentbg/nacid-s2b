using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.Suppliers;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Suppliers
{
    public class SupplierDto : Entity, IValidate
    {
        public SupplierType Type { get; set; }

        public int? InstitutionId { get; set; }
        public InstitutionDto Institution { get; set; }

        public int? ComplexId { get; set; }
        public ComplexDto Complex { get; set; }

        public SupplierRepresentativeDto Representative { get; set; }
        public List<SupplierOfferingDto> SupplierOfferings { get; set; } = new List<SupplierOfferingDto>();

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            if (Type != SupplierType.Institution && Type != SupplierType.Complex)
            {
                domainValidatorService.ThrowErrorMessage(SupplierErrorCode.Supplier_Type_Required);
            }

            if (Type == SupplierType.Institution && !InstitutionId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SupplierErrorCode.Supplier_Institution_Required);
            }

            if (Type == SupplierType.Complex && !ComplexId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(SupplierErrorCode.Supplier_Complex_Required);
            }
        }
    }
}
