import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { ReceivedVoucherHistoryFilterDto } from "../filter-dtos/received-voucher-history-filter.dto";
import { ReceivedVoucherHistoryDto } from "../dtos/received-voucher-history.dto";

@Injectable()
export class ReceivedVoucherHistoryResource {

    url = 'api/receivedVouchers';

    constructor(
        private http: HttpClient
    ) {
    }

    getSearchResult(filter: ReceivedVoucherHistoryFilterDto): Observable<SearchResultDto<ReceivedVoucherHistoryDto>> {
        return this.http.post<SearchResultDto<ReceivedVoucherHistoryDto>>(`${this.url}/${filter.receivedVoucherId}/history/search`, filter);
    }
}