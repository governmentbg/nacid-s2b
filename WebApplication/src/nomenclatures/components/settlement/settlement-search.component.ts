import { Component } from '@angular/core';
import { BaseNomenclatureSearchComponent } from '../base/base-nomenclature-search.component';
import { SettlementDto } from 'src/nomenclatures/dtos/settlements/settlement.dto';
import { NomenclatureResource } from 'src/nomenclatures/nomenclature.resource';
import { SearchUnsubscriberService } from 'src/shared/services/search-unsubscriber/search-unsubscriber.service';
import { TranslateService } from '@ngx-translate/core';
import { SettlementChangeService } from 'src/shared/services/settlement-change/settlement-change.service';
import { SettlementFilterDto } from 'src/nomenclatures/filter-dtos/settlement-filter.dto';

@Component({
  selector: 'settlement-search',
  templateUrl: './settlement-search.component.html',
  providers: [
    NomenclatureResource,
    SearchUnsubscriberService
  ]
})
export class SettlementSearchComponent extends BaseNomenclatureSearchComponent<SettlementDto, SettlementFilterDto, NomenclatureResource<SettlementDto, SettlementFilterDto>> {

  constructor(
    protected override resource: NomenclatureResource<SettlementDto, SettlementFilterDto>,
    protected override searchUnsubscriberService: SearchUnsubscriberService,
    public translateService: TranslateService,
    public settlementChangeService: SettlementChangeService,
  ) {
    super(resource, SettlementFilterDto, "settlements", searchUnsubscriberService);
  }
}
