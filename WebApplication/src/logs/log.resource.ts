import { Injectable } from "@angular/core";
import { BaseLogDto } from "./dtos/base/base-log.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { HttpClient } from "@angular/common/http";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";

@Injectable()
export class LogsResource<TLog extends BaseLogDto, TFilter extends FilterDto> {

    url = 'api/logs/';

    constructor(
        private http: HttpClient
    ) { }

    init(childUrl: string) {
        this.url = `${this.url}${childUrl}`;
    }

    getAll(filter: TFilter) {
        return this.http.post<SearchResultDto<TLog>>(`${this.url}`, filter);
    }

    getAllCount(filter: TFilter) {
        return this.http.post<number>(`${this.url}/count`, filter);
    }

    getById(id: number) {
        return this.http.get<TLog>(`${this.url}/${id}`);
    }
}