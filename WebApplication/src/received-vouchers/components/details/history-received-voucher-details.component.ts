import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { ReceivedVoucherHistoryDto } from "src/received-vouchers/dtos/received-voucher-history.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { ReceivedVoucherHistoryFilterDto } from "src/received-vouchers/filter-dtos/received-voucher-history-filter.dto";
import { ReceivedVoucherHistoryResource } from "src/received-vouchers/resources/received-voucher-history.resource";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";

@Component({
    selector: 'history-voucher-details',
    templateUrl: './history-received-voucher-details.component.html',
})
export class HistoryReceivedVoucherDetailsComponent {

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        this.close();
    }

    loadingData = false;

    receivedVoucherState = ReceivedVoucherState;

    filter: ReceivedVoucherHistoryFilterDto = new ReceivedVoucherHistoryFilterDto();
    searchResultHistories: SearchResultDto<ReceivedVoucherHistoryDto>;

    constructor(
        private activeModal: NgbActiveModal,
        private receivedVoucherHistoryResource: ReceivedVoucherHistoryResource
    ) {
    }

    ngOnInit(): void {
        this.loadingData = true;
        this.receivedVoucherHistoryResource.getSearchResult(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.searchResultHistories = e;
                this.loadingData = false;
            });
    }

    close() {
        this.activeModal.close();
    }
}