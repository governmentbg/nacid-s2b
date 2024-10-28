import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { catchError, throwError } from "rxjs";
import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { SupplierRootGroupDto } from "src/suppliers/dtos/search/supplier-group/supplier-root-group.dto";
import { SupplierOfferingSmartSpecializationFilterDto } from "src/suppliers/filter-dtos/junctions/supplier-offering-smart-specialization-filter.dto";
import { SupplierSearchGroupResource } from "src/suppliers/resources/supplier-search-group.resource";

@Component({
    selector: 'supplier-search-group',
    templateUrl: './supplier-search-group.component.html',
    styleUrls: ['./supplier-search-group.styles.css'],
    providers: [
        SearchUnsubscriberService
    ]
})
export class SupplierSearchGroupComponent implements OnInit {
    loadingData = false;
    getDataPending = false;

    searchResult: SearchResultDto<SupplierRootGroupDto> = new SearchResultDto<SupplierRootGroupDto>();
    filter: SupplierOfferingSmartSpecializationFilterDto = new SupplierOfferingSmartSpecializationFilterDto();

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.getData(false);
    }

    constructor(
        public pageHandlingService: PageHandlingService,
        public translateService: TranslateService,
        private route: ActivatedRoute,
        private resource: SupplierSearchGroupResource,
        private searchUnsubscriberService: SearchUnsubscriberService
    ) {
    }

    getData(triggerLoadingDataIndicator: boolean) {
        this.unsubscribe(1);

        if (triggerLoadingDataIndicator) {
            this.loadingData = true;
        } else {
            this.getDataPending = true;
        }

        this.filter.offset = (this.filter.currentPage - 1) * this.filter.limit;

        var subscriber = this.resource
            .getSupplierRootGroupDto(this.filter)
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

    private unsubscribe(searchType: number) {
        this.loadingData = false;
        this.getDataPending = false;
        this.searchUnsubscriberService.unsubscribeByType(searchType);
    }

    ngOnInit() {
        this.route.queryParams.subscribe(p => {
            if (p['districtId'] && p['districtName']) {
                this.filter.districtId = p['districtId'];
                this.filter.district = new DistrictDto();
                this.filter.district.name = p['districtName'];
            }

            return this.getData(true);
        });
    }
}