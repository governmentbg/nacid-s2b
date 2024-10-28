import { HttpErrorResponse } from "@angular/common/http";
import { Directive, HostListener, OnInit } from "@angular/core";
import { catchError, throwError } from "rxjs";
import { NomenclatureDto } from "src/nomenclatures/dtos/nomenclature.dto";
import { NomenclatureFilterDto } from "src/nomenclatures/filter-dtos/nomenclature-filter.dto";
import { NomenclatureResource } from "src/nomenclatures/nomenclature.resource";
import { SearchResultDto } from "src/shared/dtos/search/search-result.dto";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";

@Directive()
export abstract class BaseNomenclatureSearchComponent<TNomenclature extends NomenclatureDto, TFilter extends NomenclatureFilterDto, TNomenclatureResource extends NomenclatureResource<TNomenclature, TFilter>> implements OnInit {

  searchResult: SearchResultDto<TNomenclature> = new SearchResultDto<TNomenclature>();
  filter: TFilter = this.initializeFilter(this.structuredFilterType);

  loadingData = false;
  moreDataPending = false;
  searchDataPending = false;
  clearDataPending = false;

  @HostListener('document:keydown.enter', ['$event']) onKeydownEnterHandler() {
    this.search();
  }

  constructor(
    protected resource: TNomenclatureResource,
    protected structuredFilterType: new () => TFilter,
    protected nomenclatureUrl: string,
    protected searchUnsubscriberService: SearchUnsubscriberService,
  ) {
    if (nomenclatureUrl) {
      this.resource.init(nomenclatureUrl);
    }
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

  ngOnInit() {
    this.getData(false, true);
  }
}
