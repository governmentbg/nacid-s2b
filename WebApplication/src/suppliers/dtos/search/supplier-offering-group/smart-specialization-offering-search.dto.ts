import { OfferingType } from "src/suppliers/enums/offering-type.enum";

export class SmartSpecializationOfferingGroupDto {
    id: number;
    name: string;

    supplierId: number;
    supplierName: string;
    supplierNameAlt: string;

    institutionId: number;
    rootInstitutionId: number;
    rootInstitutionShortName: string;
    rootInstitutionShortNameAlt: string;

    shortDescription: string;
    offeringType: OfferingType;
}


