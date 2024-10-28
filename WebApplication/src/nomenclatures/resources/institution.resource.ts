import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { InstitutionDto } from "../dtos/institutions/institution.dto";

@Injectable()
export class InstitutionResource {

    url = 'api/nomenclatures/institutions';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(id: number) {
        return this.http.get<InstitutionDto>(`${this.url}/${id}`);
    }

    getSubordinates(parentId: number) {
        return this.http.get<InstitutionDto[]>(`${this.url}/${parentId}/subordinates`);
    }
}