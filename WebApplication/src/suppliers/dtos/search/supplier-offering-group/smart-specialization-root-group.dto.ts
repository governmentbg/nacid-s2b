import { SmartSpecializationGroupDto } from "./smart-specialization-group.dto";

export class SmartSpecializationRootGroupDto {
    id: number;

    code: string;
    name: string;
    nameAlt: string;
    viewOrder: number;

    smartSpecializations: SmartSpecializationGroupDto[] = [];
}