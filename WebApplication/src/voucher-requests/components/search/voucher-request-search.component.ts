import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { catchError, throwError } from "rxjs";
import { voucherRequestReadPermission } from "src/auth/constants/permission.constants";
import { UserContextService } from "src/auth/services/user-context.service";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { VoucherRequestDto } from "src/voucher-requests/dtos/voucher-request.dto";
import { VoucherRequestState } from "src/voucher-requests/enums/voucher-request-state.enum";
import { VoucherRequestFilterDto } from "src/voucher-requests/filter-dtos/voucher-request-filter.dto";
import { VoucherRequestResource } from "src/voucher-requests/resources/voucher-request.resource";

@Component({
    selector: 'voucher-request-search',
    templateUrl: './voucher-request-search.component.html',
    providers: [
        SearchUnsubscriberService
    ]
})
export class VoucherRequestSearchComponent implements OnInit {

    loadingData = false;
    searchDataPending = false;
    clearDataPending = false;

    voucherRequestState = VoucherRequestState;

    searchResult: SearchResultDto<VoucherRequestDto> = new SearchResultDto<VoucherRequestDto>();
    filter = new VoucherRequestFilterDto();

    hasNacidAlias = false;
    hasPniiditAlias = false;

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.search();
    }

    constructor(
        private resource: VoucherRequestResource,
        private searchUnsubscriberService: SearchUnsubscriberService,
        private pageHandlingService: PageHandlingService,
        private router: Router,
        public translateService: TranslateService,
        public userContextService: UserContextService
    ) {
    }

    clear() {
        this.filter = new VoucherRequestFilterDto();
        this.loadingData = false;
        this.clearDataPending = true;
        this.searchDataPending = false;
        return this.getData(false, true);
    }

    search() {
        this.loadingData = false;
        this.clearDataPending = false;
        this.searchDataPending = true;
        this.getData(false, true);
    }

    getData(triggerLoadingDataIndicator: boolean, reloadCurrentPage: boolean) {
        this.searchUnsubscriberService.unsubscribeByType(1);

        this.loadingData = triggerLoadingDataIndicator;

        if (reloadCurrentPage) {
            this.filter.currentPage = 1;
        }

        this.filter.offset = (this.filter.currentPage - 1) * this.filter.limit;

        var subscriber = this.resource
            .getSearchResultDto(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    this.searchDataPending = false;
                    this.clearDataPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
                this.searchDataPending = false;
                this.clearDataPending = false;
            });

        this.searchUnsubscriberService.addSubscription(1, subscriber);
        return subscriber;
    }

    getVoucherRequest(id: number) {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/voucherRequests', id]);
    }

    ngOnInit() {
        this.hasNacidAlias = this.userContextService.isNacid(voucherRequestReadPermission);
        this.hasPniiditAlias = this.userContextService.isPniidit(voucherRequestReadPermission);

        return this.getData(true, true);
    }
}