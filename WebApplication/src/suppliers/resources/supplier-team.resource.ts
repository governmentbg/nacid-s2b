import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { SupplierTeamDto } from "../dtos/supplier-team.dto";
import { Observable } from "rxjs";
import { IsActiveDto } from "src/shared/dtos/is-active.dto";

@Injectable()
export class SupplierTeamResource {

    url = 'api/suppliers';

    constructor(
        private http: HttpClient
    ) {
    }

    getById(supplierId: number, id: number) {
        return this.http.get<SupplierTeamDto>(`${this.url}/${supplierId}/teams/${id}`);
    }

    getBySupplier(supplierId: number) {
        return this.http.get<SupplierTeamDto[]>(`${this.url}/${supplierId}/teams`);
    }

    create(supplierId: number, supplierTeamDto: SupplierTeamDto): Observable<SupplierTeamDto> {
        return this.http.post<SupplierTeamDto>(`${this.url}/${supplierId}/teams`, supplierTeamDto);
    }

    update(supplierId: number, supplierTeamDto: SupplierTeamDto): Observable<SupplierTeamDto> {
        return this.http.put<SupplierTeamDto>(`${this.url}/${supplierId}/teams`, supplierTeamDto);
    }

    changeIsActive(supplierId: number, isActiveDto: IsActiveDto): Observable<boolean> {
        return this.http.put<boolean>(`${this.url}/${supplierId}/teams/isActive`, isActiveDto);
    }

    delete(supplierId: number, id: number): Observable<void> {
        return this.http.delete<void>(`${this.url}/${supplierId}/teams/${id}`);
    }
}