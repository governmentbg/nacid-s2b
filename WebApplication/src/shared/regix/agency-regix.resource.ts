import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CompanyDto } from "src/companies/dtos/company.dto";

@Injectable()
export class AgencyRegixResource {

    url = 'api/regix/agency';

    constructor(
        private http: HttpClient
    ) {
    }

    getCompanyFromAgencyRegix(uic: string): Observable<CompanyDto> {
        return this.http.get<CompanyDto>(`${this.url}/company/${uic}`);
    }

}