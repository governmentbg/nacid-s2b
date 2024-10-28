import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { Observable } from "rxjs";
import { SupplierOfferingSmartSpecializationFilterDto } from "../filter-dtos/junctions/supplier-offering-smart-specialization-filter.dto";
import { SupplierRootGroupDto } from "../dtos/search/supplier-group/supplier-root-group.dto";

@Injectable()
export class SupplierSearchGroupResource {

  url = 'api/supplierGroup';

  constructor(
    private http: HttpClient
  ) {
  }

  getSupplierRootGroupDto(filter: SupplierOfferingSmartSpecializationFilterDto): Observable<SearchResultDto<SupplierRootGroupDto>> {
    return this.http.post<SearchResultDto<SupplierRootGroupDto>>(`${this.url}/search`, filter);
  }

}