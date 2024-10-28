export class FilterResultDto {
    id: number;
    code: string;
    name: string;
    nameAlt: string;
    shortName: string;
    shortNameAlt: string;
    count: number;

    // Client only
    isSelected = false;
}
