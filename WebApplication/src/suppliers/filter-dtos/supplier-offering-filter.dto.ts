import { KeywordsSearchType } from "src/shared/enums/keywords-search-type.enum";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class SupplierOfferingFilterDto extends FilterDto {
    supplierId: number;
    name: string;
    keywordsSearchType = KeywordsSearchType.exactMatch;
    keywords: string;

    supplierIds: number[] = [];
    smartSpecializationRootIds: number[] = [];

    override limit = 10;
}