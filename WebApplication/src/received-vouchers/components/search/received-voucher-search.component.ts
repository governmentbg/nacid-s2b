import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { catchError, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { receivedVoucherCreatePermission, receivedVoucherReadPermission } from "src/auth/constants/permission.constants";
import { PermissionService } from "src/auth/services/permission.service";
import { UserContextService } from "src/auth/services/user-context.service";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { ReceivedVoucherFilterDto } from "src/received-vouchers/filter-dtos/received-voucher-filter.dto";
import { ReceivedVoucherResource } from "src/received-vouchers/resources/received-voucher.resource";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";

@Component({
    selector: 'received-voucher-search',
    templateUrl: './received-voucher-search.component.html',
    providers: [
        SearchUnsubscriberService
    ]
})
export class ReceivedVoucherSearchComponent implements OnInit {

    loadingData = false;
    searchDataPending = false;
    clearDataPending = false;
    isNacidOrPniiditAlias = false;
    isCompanyUser = false;

    receivedVoucherState = ReceivedVoucherState;

    searchResult: SearchResultDto<ReceivedVoucherDto> = new SearchResultDto<ReceivedVoucherDto>();
    filter = new ReceivedVoucherFilterDto();

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.search();
    }

    constructor(
        private resource: ReceivedVoucherResource,
        private searchUnsubscriberService: SearchUnsubscriberService,
        private pageHandlingService: PageHandlingService,
        private router: Router,
        private permissionService: PermissionService,
        public configuration: Configuration,
        public translateService: TranslateService,
        public userContextService: UserContextService
    ) {
    }

    clear() {
        this.filter = new ReceivedVoucherFilterDto();
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

    changedSupplier(supplierId: number) {
        this.filter.offering = null;
        this.filter.offeringId = null;
        this.filter.supplierId = supplierId;
    }

    changedOffering(offering: SupplierOfferingDto) {
        this.filter.offering = offering;
        this.filter.offeringId = offering?.id;
        this.filter.supplier = offering?.supplier;
        this.filter.supplierId = offering?.supplierId;
    }

    add() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/receivedVouchers/create']);
    }

    ngOnInit() {
        this.isNacidOrPniiditAlias = this.userContextService.isNacid(receivedVoucherReadPermission) || this.userContextService.isPniidit(receivedVoucherReadPermission);
        this.isCompanyUser = this.permissionService.verifyIsCompanyUserWithPermission(receivedVoucherCreatePermission);

        return this.getData(true, true);
    }
}