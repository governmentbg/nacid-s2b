import { SupplierOfferingDto } from "../supplier-offering.dto";

export class SupplierSearchDto {
    id: number;

    // Common
    name: string;
    nameAlt: string;

    // Institution
    uic: string;
    rootId: number;
    rootName: string;
    rootNameAlt: string;

    supplierOfferings: SupplierOfferingDto[] = [];
}