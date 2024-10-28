import { SmartSpecializationGroupDto } from "../../supplier-offering-group/smart-specialization-group.dto";

export class BaseSupplierGroupDto {
    id: number;

    // Institution
    uic: string;
    code: string;

    // Common
    name: string;
    nameAlt: string;
    viewOrder: number

    smartSpecializations: SmartSpecializationGroupDto[] = [];
}