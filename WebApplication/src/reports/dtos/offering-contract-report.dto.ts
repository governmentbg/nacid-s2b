import { SupplierType } from "src/suppliers/enums/supplier-type.enum";

export class OfferingContractReportDto {
    supplierId: number;
    supplierType: SupplierType;
    supplierName: string;

    institutionRootName: string;
    institutionRootId: number;
    institutionId: number;
    complexId: number;

    offeringId: number;
    offeringCode: string;
    offeringName: string;

    offeringsCount: number;
}