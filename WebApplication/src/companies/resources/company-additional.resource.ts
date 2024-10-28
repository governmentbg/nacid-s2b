import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CompanyAdditionalDto } from "../dtos/company-additional.dto";
import { Observable } from "rxjs";

@Injectable()
export class CompanyAdditionalResource {

    url = 'api/companies';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(companyId: number) {
        return this.http.get<CompanyAdditionalDto>(`${this.url}/${companyId}/additionals`);
    }

    create(companyId: number, companyAdditionalDto: CompanyAdditionalDto): Observable<CompanyAdditionalDto> {
        return this.http.post<CompanyAdditionalDto>(`${this.url}/${companyId}/additionals`, companyAdditionalDto);
    }

    update(companyId: number, companyAdditionalDto: CompanyAdditionalDto): Observable<CompanyAdditionalDto> {
        return this.http.put<CompanyAdditionalDto>(`${this.url}/${companyId}/additionals`, companyAdditionalDto);
    }
}