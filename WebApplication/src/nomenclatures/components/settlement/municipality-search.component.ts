import { Component } from '@angular/core';
import { BaseNomenclatureSearchComponent } from '../base/base-nomenclature-search.component';
import { MunicipalityDto } from 'src/nomenclatures/dtos/settlements/municipality.dto';
import { MunicipalityFilterDto } from 'src/nomenclatures/filter-dtos/municipality-filter.dto';
import { NomenclatureResource } from 'src/nomenclatures/nomenclature.resource';
import { SearchUnsubscriberService } from 'src/shared/services/search-unsubscriber/search-unsubscriber.service';
import { TranslateService } from '@ngx-translate/core';
import { SettlementChangeService } from 'src/shared/services/settlement-change/settlement-change.service';

@Component({
  selector: 'municipality-search',
  templateUrl: './municipality-search.component.html',
  providers: [
    NomenclatureResource,
    SearchUnsubscriberService
  ]
})
export class MunicipalitySearchComponent extends BaseNomenclatureSearchComponent<MunicipalityDto, MunicipalityFilterDto, NomenclatureResource<MunicipalityDto, MunicipalityFilterDto>> {

  constructor(
    protected override resource: NomenclatureResource<MunicipalityDto, MunicipalityFilterDto>,
    protected override searchUnsubscriberService: SearchUnsubscriberService,
    public translateService: TranslateService,
    public settlementChangeService: SettlementChangeService,
  ) {
    super(resource, MunicipalityFilterDto, "municipalities", searchUnsubscriberService);
  }
}
