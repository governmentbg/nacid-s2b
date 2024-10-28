import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IsActiveDto } from "src/shared/dtos/is-active.dto";
import { SupplierOfferingDto } from "../dtos/supplier-offering.dto";

@Injectable()
export class SupplierOfferingResource {

    url = 'api/suppliers';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(supplierId: number, id: number) {
        return this.http.get<SupplierOfferingDto>(`${this.url}/${supplierId}/offerings/${id}`);
    }

    getBySupplier(supplierId: number) {
        return this.http.get<SupplierOfferingDto[]>(`${this.url}/${supplierId}/offerings`);
    }

    create(supplierId: number, supplierOfferingDto: SupplierOfferingDto): Observable<SupplierOfferingDto> {
        return this.http.post<SupplierOfferingDto>(`${this.url}/${supplierId}/offerings`, supplierOfferingDto);
    }

    update(supplierId: number, supplierOfferingDto: SupplierOfferingDto): Observable<SupplierOfferingDto> {
        return this.http.put<SupplierOfferingDto>(`${this.url}/${supplierId}/offerings`, supplierOfferingDto);
    }

    delete(supplierId: number, id: number): Observable<void> {
        return this.http.delete<void>(`${this.url}/${supplierId}/offerings/${id}`);
    }

    changeIsActive(supplierId: number, isActiveDto: IsActiveDto): Observable<boolean> {
        return this.http.put<boolean>(`${this.url}/${supplierId}/offerings/isActive`, isActiveDto);
    }
}