import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { SmartSpecializationRootGroupDto } from "../dtos/search/supplier-offering-group/smart-specialization-root-group.dto";
import { SupplierOfferingFilterDto } from "../filter-dtos/supplier-offering-filter.dto";
import { FilterResultGroupDto } from "src/shared/dtos/search/filter-result-group.dto";
import { SupplierOfferingSearchDto } from "../dtos/search/supplier-offering-group/supplier-offering-search.dto";

@Injectable()
export class SupplierOfferingGroupResource {

    url = 'api/supplierOfferings/group';

    constructor(
        private http: HttpClient
    ) {
    }

    getSmartSpecializations(): Observable<SearchResultDto<SmartSpecializationRootGroupDto>> {
        return this.http.get<SearchResultDto<SmartSpecializationRootGroupDto>>(`${this.url}/smartSpecializations`);
    }

    getSupplierOfferings(filter: SupplierOfferingFilterDto): Observable<FilterResultGroupDto<SupplierOfferingSearchDto>> {
        return this.http.post<FilterResultGroupDto<SupplierOfferingSearchDto>>(`${this.url}`, filter);
    }
}