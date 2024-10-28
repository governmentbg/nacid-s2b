import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { Observable } from "rxjs";
import { ApproveRegistrationFilterDto } from "./filter-dtos/approve-registration-filter.dto";
import { ApproveRegistrationSearchDto } from "./dtos/search/approve-registration-search.dto";
import { DeclineRegistrationDto } from "./dtos/decline-registration.dto";
import { ApproveRegistrationDto } from "./dtos/approve-registration.dto";
import { SupplierOfferingCountDto } from "./dtos/supplier-offering-count.dto";

@Injectable()
export class ApproveRegistrationsResource {

  url = 'api/approveRegistrations';

  constructor(
    private http: HttpClient
  ) { }

  getById(id: number): Observable<ApproveRegistrationSearchDto> {
    return this.http.get<ApproveRegistrationSearchDto>(`${this.url}/${id}`);
  }

  getSearchResultDto(filter: ApproveRegistrationFilterDto): Observable<SearchResultDto<ApproveRegistrationSearchDto>> {
    return this.http.post<SearchResultDto<ApproveRegistrationSearchDto>>(`${this.url}`, filter);
  }

  declineRegistration(declineRegistrationDto: DeclineRegistrationDto): Observable<ApproveRegistrationSearchDto> {
    return this.http.put<ApproveRegistrationSearchDto>(`${this.url}/decline`, declineRegistrationDto);
  }

  approveRegistration(approveRegistrationDto: ApproveRegistrationDto): Observable<ApproveRegistrationSearchDto> {
    return this.http.put<ApproveRegistrationSearchDto>(`${this.url}/approve`, approveRegistrationDto);
  }

  supplierOfferingCount(filter: ApproveRegistrationFilterDto): Observable<SupplierOfferingCountDto> {
    return this.http.post<SupplierOfferingCountDto>(`${this.url}/count`, filter);
  }

  signUpSupplierEdit(approveRegistration: ApproveRegistrationSearchDto): Observable<void> {
    return this.http.put<void>(`${this.url}/signUpSupplier`, approveRegistration);
  }

  updateRepresentativeInfo(approveRegistration: ApproveRegistrationSearchDto): Observable<void> {
    return this.http.put<void>(`${this.url}/updateRepresentativeInfo`, approveRegistration);
  }
}



