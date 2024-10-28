import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SupplierEquipmentDto } from "../dtos/supplier-equipment.dto";

@Injectable()
export class SupplierEquipmentResource {

    url = 'api/suppliers';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(supplierId: number, id: number) {
        return this.http.get<SupplierEquipmentDto>(`${this.url}/${supplierId}/equipment/${id}`);
    }

    getBySupplier(supplierId: number) {
        return this.http.get<SupplierEquipmentDto[]>(`${this.url}/${supplierId}/equipment`);
    }

    create(supplierId: number, supplierEquipmentDto: SupplierEquipmentDto): Observable<SupplierEquipmentDto> {
        return this.http.post<SupplierEquipmentDto>(`${this.url}/${supplierId}/equipment`, supplierEquipmentDto);
    }

    update(supplierId: number, supplierEquipmentDto: SupplierEquipmentDto): Observable<SupplierEquipmentDto> {
        return this.http.put<SupplierEquipmentDto>(`${this.url}/${supplierId}/equipment`, supplierEquipmentDto);
    }

    delete(supplierId: number, id: number): Observable<void> {
        return this.http.delete<void>(`${this.url}/${supplierId}/equipment/${id}`);
    }
}