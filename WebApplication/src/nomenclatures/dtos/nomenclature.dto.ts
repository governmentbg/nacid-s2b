export class NomenclatureDto {
    id: number;
    code: string;
    name: string;
    nameAlt: string;
    description: string;
    descriptionAlt: string;
    isActive: boolean = true;
}