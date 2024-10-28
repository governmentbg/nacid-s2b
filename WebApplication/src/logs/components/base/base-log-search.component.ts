import { HttpErrorResponse } from "@angular/common/http";
import { Directive, HostListener, OnInit } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { BaseLogDto } from "src/logs/dtos/base/base-log.dto";
import { LogsResource } from "src/logs/log.resource";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";

@Directive()
export abstract class BaseLogSearchComponent<TLog extends BaseLogDto, TFilter extends FilterDto> implements OnInit {

    searchResult: SearchResultDto<TLog> = new SearchResultDto<TLog>();
    filter: TFilter = this.initializeFilter(this.structuredFilterType);

    loadingData = false;
    moreDataPending = false;
    searchDataPending = false;
    clearDataPending = false;
    dataCountPending = false;
    
    dataCount: number = null;

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.search();
    }

    constructor(
        protected resource: LogsResource<TLog, TFilter>,
        protected structuredFilterType: new () => TFilter,
        protected logUrl: string,
        protected searchUnsubscriberService: SearchUnsubscriberService,
        protected modalService: NgbModal,
        public configuration: Configuration
    ) {
        this.resource.init(logUrl);
    }

    initializeFilter(C: { new(): TFilter }) {
        return new C();
    }

    clear() {
        this.filter = this.initializeFilter(this.structuredFilterType);
        this.loadingData = false;
        this.moreDataPending = false;
        this.searchDataPending = false;
        this.clearDataPending = true;
        return this.getData(false, false);
    }

    search() {
        this.loadingData = false;
        this.moreDataPending = false;
        this.clearDataPending = false;
        this.searchDataPending = true;
        this.getData(false, false);
    }

    loadMore() {
        this.loadingData = false;
        this.clearDataPending = false;
        this.searchDataPending = false;
        this.moreDataPending = true;
        this.getData(true, false);
    }

    private getData(loadMore: boolean, triggerLoadingDataIndicator = true) {
        this.searchUnsubscriberService.unsubscribeByType(1);
        this.loadingData = triggerLoadingDataIndicator;
        this.dataCount = null;

        if (loadMore) {
            this.filter.limit = this.filter.limit + this.searchResult.result.length;
        } else {
            this.filter.limit = 30;
        }

        var subscriber = this.resource
            .getAll(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    this.moreDataPending = false;
                    this.clearDataPending = false;
                    this.searchDataPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
                this.moreDataPending = false;
                this.clearDataPending = false;
                this.searchDataPending = false;
            });

        this.searchUnsubscriberService.addSubscription(1, subscriber);
        return subscriber;
    }

    getDataCount() {
        this.searchUnsubscriberService.unsubscribeByType(2);
        this.dataCountPending = true;

        var subscriber = this.resource
            .getAllCount(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.dataCountPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.dataCount = e;
                this.dataCountPending = false;
            });

        this.searchUnsubscriberService.addSubscription(2, subscriber);
        return subscriber;
    }

    ngOnInit() {
        return this.getData(false, true);
    }
}