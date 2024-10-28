import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class SupplierFilterDto extends FilterDto {
    name: string;

    hasSupplierOfferings: boolean;
}