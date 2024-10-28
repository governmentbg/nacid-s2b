import { SupplierOfferingFileDto } from "../../supplier-offering-file.dto";

export class SupplierOfferingSearchDto {
    id: number;

    code: string;

    name: string;
    nameAlt: string;
    shortDescription: string;

    representativeUserName: string;
    representativeName: string;
    representativeNameAlt: string;
    representativePhoneNumber: string;

    file: SupplierOfferingFileDto;

    supplierId: number;
    supplierName: string;
    supplierNameAlt: string;

    // Institution
    institutionId: number;
    rootInstitutionId: number;
    rootInstitutionShortName: string;
    rootInstitutionShortNameAlt: string;
}