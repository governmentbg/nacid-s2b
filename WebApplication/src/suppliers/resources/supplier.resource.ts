import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { SupplierDto } from "../dtos/supplier.dto";

@Injectable()
export class SupplierResource {

    url = 'api/suppliers';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(id: number) {
        return this.http.get<SupplierDto>(`${this.url}/${id}`);
    }
}