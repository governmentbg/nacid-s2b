import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { CompanyDto } from "../dtos/company.dto";
import { CompanyFilterDto } from "../filter-dtos/company-filter.dto";
import { IsActiveDto } from "src/shared/dtos/is-active.dto";

@Injectable()
export class CompanyResource {

    url = 'api/companies';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(id: number) {
        return this.http.get<CompanyDto>(`${this.url}/${id}`);
    }

    getSearchResultDto(filter: CompanyFilterDto): Observable<SearchResultDto<CompanyDto>> {
        return this.http.post<SearchResultDto<CompanyDto>>(`${this.url}`, filter);
    }

    changeIsActive(isActiveDto: IsActiveDto): Observable<boolean> {
        return this.http.put<boolean>(`${this.url}/isActive`, isActiveDto);
    }
}