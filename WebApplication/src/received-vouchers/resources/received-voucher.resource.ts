import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ReceivedVoucherFilterDto } from "../filter-dtos/received-voucher-filter.dto";
import { Observable } from "rxjs";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { ReceivedVoucherDto } from "../dtos/received-voucher.dto";

@Injectable()
export class ReceivedVoucherResource {

    url = 'api/receivedVouchers';

    constructor(
        private http: HttpClient
    ) {
    }

    getSearchResultDto(filter: ReceivedVoucherFilterDto): Observable<SearchResultDto<ReceivedVoucherDto>> {
        return this.http.post<SearchResultDto<ReceivedVoucherDto>>(`${this.url}/search`, filter);
    }

    getById(id: number): Observable<ReceivedVoucherDto> {
        return this.http.get<ReceivedVoucherDto>(`${this.url}/${id}`);
    }

    create(receivedVoucher: ReceivedVoucherDto): Observable<void> {
        return this.http.post<void>(`${this.url}`, receivedVoucher);
    }

    update(receivedVoucher: ReceivedVoucherDto): Observable<ReceivedVoucherDto> {
        return this.http.put<ReceivedVoucherDto>(`${this.url}`, receivedVoucher);
    }

    terminate(id: number): Observable<ReceivedVoucherDto> {
        return this.http.get<ReceivedVoucherDto>(`${this.url}/${id}/terminate`)
    }
}