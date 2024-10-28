import { SmartSpecializationOfferingGroupDto } from "./smart-specialization-offering-search.dto";

export class SmartSpecializationGroupDto {
    id: number;

    code: string;
    name: string;
    nameAlt: string;
    viewOrder: number;

    offeringsCount: number;
    offerings: SmartSpecializationOfferingGroupDto[] = [];
}