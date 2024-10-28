import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class NomenclatureFilterDto extends FilterDto {
    code: string;
    name: string;
    description: string;
    aliases: string[] = [];
    excludeAliases = true;
}