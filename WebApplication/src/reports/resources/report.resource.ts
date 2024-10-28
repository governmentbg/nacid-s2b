import { Injectable } from "@angular/core";
import { DapperFilterDto } from "../filter-dtos/base/dapper-filter.dto";
import { HttpClient } from "@angular/common/http";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { Observable, map } from "rxjs";

@Injectable()
export class ReportResource<T extends any, TFilter extends DapperFilterDto> {

    url = 'api/report';

    constructor(
        protected http: HttpClient
    ) { }

    init(childUrl: string) {
        this.url = `${this.url}/${childUrl}`;
    }

    getReport(filter: TFilter): Observable<SearchResultDto<T>> {
        return this.http.post<SearchResultDto<T>>(`${this.url}`, filter);
    }

    exportExcel(filter: TFilter): Observable<Blob> {
        return this.http.post(`${this.url}/excel`, filter, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }))
            );
    }

    exportJson(filter: TFilter): Observable<Blob> {
        return this.http.post(`${this.url}/json`, filter, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([response], { type: 'application/json' }))
            );
    }

    exportCsv(filter: TFilter): Observable<Blob> {
        return this.http.post(`${this.url}/csv`, filter, { responseType: 'blob' })
            .pipe(
                map(response => new Blob([new Uint8Array([0xEF, 0xBB, 0xBF]), response], { type: 'text/csv;charset=utf-8' }))
            );
    }
}