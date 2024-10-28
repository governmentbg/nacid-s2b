import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { FilterResultGroupDto } from "src/shared/dtos/search/filter-result-group.dto";
import { KeywordsSearchType } from "src/shared/enums/keywords-search-type.enum";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { SupplierOfferingSearchDto } from "src/suppliers/dtos/search/supplier-offering-group/supplier-offering-search.dto";
import { ClearFilterType } from "src/suppliers/enums/clear-filter-type.enum";
import { ClearFilterDto } from "src/suppliers/filter-dtos/clear-filter.dto";
import { SupplierOfferingFilterDto } from "src/suppliers/filter-dtos/supplier-offering-filter.dto";
import { SupplierOfferingGroupResource } from "src/suppliers/resources/supplier-offering-group.resource";

@Component({
    selector: 'supplier-offering-search',
    templateUrl: './supplier-offering-search.component.html',
    styleUrls: ['./supplier-offering-search.styles.css'],
    providers: [
        SearchUnsubscriberService
    ]
})
export class SupplierOfferingSearchComponent implements OnInit {

    loadingData = false;
    getDataPending = false;
    showOfferingGroup = false;

    keywordsSearchType = KeywordsSearchType;

    clearFilterDtos: ClearFilterDto[] = [];
    filterResultGroup: FilterResultGroupDto<SupplierOfferingSearchDto> = new FilterResultGroupDto<SupplierOfferingSearchDto>();
    filter = new SupplierOfferingFilterDto();

    @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
        this.getData(false);
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private resource: SupplierOfferingGroupResource,
        private searchUnsubscriberService: SearchUnsubscriberService
    ) {
        this.filter.keywordsSearchType = KeywordsSearchType.matchAny;
    }

    getData(triggerLoadingDataIndicator: boolean) {
        this.unsubscribe(1);

        if (triggerLoadingDataIndicator) {
            this.loadingData = true;
        } else {
            const nameFilter = this.clearFilterDtos.find(e => e.clearType === ClearFilterType.name);
            const keywordsFilter = this.clearFilterDtos.find(e => e.clearType === ClearFilterType.keywords);

            if (this.filter.name) {
                if (nameFilter) {
                    nameFilter.value = this.filter.name;
                } else {
                    let nameFilter = new ClearFilterDto();
                    nameFilter.clearType = ClearFilterType.name;
                    nameFilter.value = this.filter.name;
                    this.clearFilterDtos.push(nameFilter);
                }
            } else if (nameFilter) {
                this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== ClearFilterType.name);
            }

            if (this.filter.keywords) {
                if (keywordsFilter) {
                    keywordsFilter.value = this.filter.keywords;
                } else {
                    let keywordsFilter = new ClearFilterDto();
                    keywordsFilter.clearType = ClearFilterType.keywords;
                    keywordsFilter.value = this.filter.keywords;
                    this.clearFilterDtos.push(keywordsFilter);
                }
            } else if (keywordsFilter) {
                this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== ClearFilterType.keywords);
            }

            this.getDataPending = true;
        }

        var subscriber = this.resource
            .getSupplierOfferings(this.filter)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    this.getDataPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.filterResultGroup = e;
                this.showOfferingGroup = true;
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
        this.showOfferingGroup = !this.activatedRoute.snapshot.queryParams['specializationGroup'];
        
        if (this.showOfferingGroup) {
            this.getData(true);
        }
    }
}