import { Component } from '@angular/core';
import { BaseNomenclatureSearchComponent } from '../base/base-nomenclature-search.component';
import { DistrictDto } from 'src/nomenclatures/dtos/settlements/district.dto';
import { NomenclatureFilterDto } from 'src/nomenclatures/filter-dtos/nomenclature-filter.dto';
import { NomenclatureResource } from 'src/nomenclatures/nomenclature.resource';
import { SearchUnsubscriberService } from 'src/shared/services/search-unsubscriber/search-unsubscriber.service';
import { SettlementChangeService } from 'src/shared/services/settlement-change/settlement-change.service';

@Component({
  selector: 'district-search',
  templateUrl: './district-search.component.html',
  providers: [
    NomenclatureResource,
    SearchUnsubscriberService
  ]
})
export class DistrictSearchComponent extends BaseNomenclatureSearchComponent<DistrictDto, NomenclatureFilterDto, NomenclatureResource<DistrictDto, NomenclatureFilterDto>> {

  constructor(
    protected override resource: NomenclatureResource<DistrictDto, NomenclatureFilterDto>,
    protected override searchUnsubscriberService: SearchUnsubscriberService,
    public settlementChangeService: SettlementChangeService,
  ) {
    super(resource, NomenclatureFilterDto, "districts", searchUnsubscriberService);
  }
}
