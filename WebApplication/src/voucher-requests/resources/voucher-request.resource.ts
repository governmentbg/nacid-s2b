import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { VoucherRequestFilterDto } from "../filter-dtos/voucher-request-filter.dto";
import { VoucherRequestDto } from "../dtos/voucher-request.dto";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { Observable } from "rxjs";
import { VoucherRequestStateDto } from "../dtos/voucher-request-state.dto";
import { ChangedStateDto } from "../dtos/changed-state.dto";

@Injectable()
export class VoucherRequestResource {

    url = 'api/voucherRequests';

    constructor(
        private http: HttpClient
    ) {
    }

    getSearchResultDto(filter: VoucherRequestFilterDto): Observable<SearchResultDto<VoucherRequestDto>> {
        return this.http.post<SearchResultDto<VoucherRequestDto>>(`${this.url}`, filter);
    }

    getById(id: number): Observable<VoucherRequestDto> {
        return this.http.get<VoucherRequestDto>(`${this.url}/${id}`);
    }

    requestCode(voucherRequestStateDto: VoucherRequestStateDto): Observable<ChangedStateDto> {
        return this.http.put<ChangedStateDto>(`${this.url}/requestCode`, voucherRequestStateDto);
    }

    generateCode(voucherRequestStateDto: VoucherRequestStateDto): Observable<ChangedStateDto> {
        return this.http.put<ChangedStateDto>(`${this.url}/generateCode`, voucherRequestStateDto);
    }

    declineCode(voucherRequestStateDto: VoucherRequestStateDto): Observable<ChangedStateDto> {
        return this.http.put<ChangedStateDto>(`${this.url}/declineCode`, voucherRequestStateDto);
    }
}