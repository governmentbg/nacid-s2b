import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { CompanyDto } from "src/companies/dtos/company.dto";
import { CompanyFilterDto } from "src/companies/filter-dtos/company-filter.dto";
import { CompanyResource } from "src/companies/resources/company.resource";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";

@Component({
    selector: 'company-search',
    templateUrl: './company-search.component.html',
    styleUrls: ['./company-search.styles.css'],
    providers: [
        SearchUnsubscriberService
    ]
})
export class CompanySearchComponent implements OnInit {

    loadingData = false;
    getDataPending = false;

    searchResult: SearchResultDto<CompanyDto> = new SearchResultDto<CompanyDto>();
    filter = new CompanyFilterDto();

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.getData(false, true);
    }

    constructor(
        private resource: CompanyResource,
        private searchUnsubscriberService: SearchUnsubscriberService,
        private pageHandlingService: PageHandlingService,
        private router: Router,
    ) {
    }

    getData(triggerLoadingDataIndicator: boolean, reloadCurrentPage: boolean) {
        this.unsubscribe(1);

        if (reloadCurrentPage) {
            this.filter.currentPage = 1;
        }

        if (triggerLoadingDataIndicator) {
            this.loadingData = true;
        } else {
            this.getDataPending = true;
        }

        this.filter.offset = (this.filter.currentPage - 1) * this.filter.limit;

        var subscriber = this.resource
            .getSearchResultDto(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    this.getDataPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
                this.getDataPending = false;
            });

        this.searchUnsubscriberService.addSubscription(1, subscriber);
        return subscriber;
    }

    getCompany(id: number) {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/companies', id]);
    }

    private unsubscribe(searchType: number) {
        this.loadingData = false;
        this.getDataPending = false;
        this.searchUnsubscriberService.unsubscribeByType(searchType);
    }

    ngOnInit() {
        return this.getData(true, true);
    }

}