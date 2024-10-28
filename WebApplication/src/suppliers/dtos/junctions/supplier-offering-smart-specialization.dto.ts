import { SmartSpecializationDto } from "src/nomenclatures/dtos/smart-specializations/smart-specialization.dto";
import { SupplierOfferingSmartSpecializationType } from "src/suppliers/enums/supplier-offering-smart-specialization-type.enum";

export class SupplierOfferingSmartSpecializationDto {
    id: number;

    supplierOfferingId: number;

    smartSpecializationId: number;
    smartSpecialization: SmartSpecializationDto;

    type: SupplierOfferingSmartSpecializationType = SupplierOfferingSmartSpecializationType.secondary;
}