import { SupplierEquipmentDto } from "../supplier-equipment.dto";
import { SupplierOfferingDto } from "../supplier-offering.dto";

export class SupplierOfferingEquipmentDto {
    id: number;

    supplierOfferingId: number;
    supplierOffering: SupplierOfferingDto;

    supplierEquipmentId: number;
    supplierEquipment: SupplierEquipmentDto;
}