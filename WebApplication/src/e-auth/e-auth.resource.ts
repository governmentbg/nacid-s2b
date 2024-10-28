import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { SamlRequestDto } from "./dtos/saml-request.dto";
import { Observable } from "rxjs";

@Injectable()
export class EAuthResource {

    url = 'api/EAuth';

    constructor(
        private http: HttpClient
    ) {
    }

    getSamlRequest(): Observable<SamlRequestDto> {
        return this.http.get<SamlRequestDto>(`${this.url}`);
    }
}