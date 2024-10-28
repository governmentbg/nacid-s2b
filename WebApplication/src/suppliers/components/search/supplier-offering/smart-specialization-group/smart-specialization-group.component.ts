import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { catchError, throwError } from "rxjs";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { SmartSpecializationRootGroupDto } from "src/suppliers/dtos/search/supplier-offering-group/smart-specialization-root-group.dto";
import { SupplierOfferingGroupResource } from "src/suppliers/resources/supplier-offering-group.resource";

@Component({
    selector: 'smart-specialization-group',
    templateUrl: './smart-specialization-group.component.html',
    providers: [
        SearchUnsubscriberService
    ]
})
export class SmartSpecializationGroupComponent implements OnInit {

    loadingData = false;
    currentPage = 1;
    pageSize = 10;

    searchResult: SearchResultDto<SmartSpecializationRootGroupDto> = new SearchResultDto<SmartSpecializationRootGroupDto>();

    constructor(
        private resource: SupplierOfferingGroupResource,
        private searchUnsubscriberService: SearchUnsubscriberService
    ) {
    }

    private getData() {
        this.unsubscribe(1);

        this.loadingData = true;

        var subscriber = this.resource
            .getSmartSpecializations()
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
            });

        this.searchUnsubscriberService.addSubscription(1, subscriber);
        return subscriber;
    }

    private unsubscribe(searchType: number) {
        this.loadingData = false;
        this.searchUnsubscriberService.unsubscribeByType(searchType);
    }

    ngOnInit() {
        return this.getData();
    }
}