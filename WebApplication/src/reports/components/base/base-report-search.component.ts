import { HttpErrorResponse } from "@angular/common/http";
import { Directive, HostListener } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { saveAs } from "file-saver-es";
import { catchError, throwError } from "rxjs";
import { DapperFilterDto } from "src/reports/filter-dtos/base/dapper-filter.dto";
import { ReportResource } from "src/reports/resources/report.resource";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";

@Directive()
export abstract class BaseReportSearchComponent<T extends any, TFilter extends DapperFilterDto> {

    loadingExport = false;
    searchDataPending = false;
    clearDataPending = false;
    searched = false;

    searchResult: SearchResultDto<T> = new SearchResultDto<T>();
    filter: TFilter = this.initializeFilter(this.structuredFilterType);
    translatedReportName: string;

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.search();
    }

    constructor(
        public pageHandlingService: PageHandlingService,
        protected resource: ReportResource<T, TFilter>,
        protected searchUnsubscriberService: SearchUnsubscriberService,
        protected translateService: TranslateService,
        protected structuredFilterType: new () => TFilter,
        protected childUrl: string,
        protected reportName: string
    ) {
        this.translateService.get(reportName)
            .subscribe(translatedTitle => this.translatedReportName = translatedTitle);
        this.resource.init(childUrl);
    }

    clear() {
        this.filter = this.initializeFilter(this.structuredFilterType);
        this.searched = false;
        this.clearDataPending = true;
        this.searchDataPending = false;
        return this.getData();
    }

    search() {
        this.searched = true;
        this.clearDataPending = false;
        this.searchDataPending = true;
        this.getData();
    }

    getData() {
        this.searchUnsubscriberService.unsubscribeByType(1);

        this.filter.currentPage = 1;

        var subscriber = this.resource
            .getReport(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.searchDataPending = false;
                    this.clearDataPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.searchResult = e;
                this.searchDataPending = false;
                this.clearDataPending = false;
            });

        this.searchUnsubscriberService.addSubscription(1, subscriber);
        return subscriber;
    }

    exportExcel() {
        if (!this.loadingExport) {
            this.loadingExport = true;
            this.resource.exportExcel(this.filter)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingExport = false;
                        return throwError(() => err);
                    })
                )
                .subscribe((blob: Blob) => {
                    saveAs(blob, this.translatedReportName + '.xlsx');
                    this.loadingExport = false;
                });
        }
    }

    exportJson() {
        if (!this.loadingExport) {
            this.loadingExport = true;
            this.resource.exportJson(this.filter)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingExport = false;
                        return throwError(() => err);
                    })
                )
                .subscribe((blob: Blob) => {
                    saveAs(blob, this.translatedReportName + '.json');
                    this.loadingExport = false;
                });
        }
    }

    exportCsv() {
        if (!this.loadingExport) {
            this.loadingExport = true;
            this.resource.exportCsv(this.filter)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingExport = false;
                        return throwError(() => err);
                    })
                )
                .subscribe((blob: Blob) => {
                    saveAs(blob, this.translatedReportName + '.csv');
                    this.loadingExport = false;
                });
        }
    }

    initializeFilter(C: { new(): TFilter }) {
        var newFilter = new C();
        return newFilter;
    }
}