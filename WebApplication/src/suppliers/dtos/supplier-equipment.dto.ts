import { SupplierOfferingEquipmentDto } from "./junctions/supplier-offering-equipment.dto";
import { SupplierEquipmentFileDto } from "./supplier-equipment-file.dto";

export class SupplierEquipmentDto {
    id: number;

    supplierId: number;

    name: string;

    description: string;

    file: SupplierEquipmentFileDto;

    supplierOfferingEquipment: SupplierOfferingEquipmentDto[] = [];
}