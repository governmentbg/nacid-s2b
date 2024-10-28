import { FilterResultDto } from "./filter-result.dto";
import { SearchResultDto } from "./search-result.dto";

export class FilterResultGroupDto<T> {
    searchResult: SearchResultDto<T> = new SearchResultDto<T>();
    filterResult: Record<string, FilterResultDto[]>;
}
