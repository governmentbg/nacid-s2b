import { Component } from '@angular/core';
import { SmartSpecializationDto } from 'src/nomenclatures/dtos/smart-specializations/smart-specialization.dto';
import { NomenclatureResource } from 'src/nomenclatures/nomenclature.resource';
import { SearchUnsubscriberService } from 'src/shared/services/search-unsubscriber/search-unsubscriber.service';
import { BaseNomenclatureSearchComponent } from '../base/base-nomenclature-search.component';
import { TranslateService } from '@ngx-translate/core';
import { SmartSpecializationFilterDto } from 'src/nomenclatures/filter-dtos/smart-specialization-filter.dto';

@Component({
  selector: 'smart-specialization-search',
  templateUrl: './smart-specialization-search.component.html',
  providers: [
    NomenclatureResource,
    SearchUnsubscriberService
  ]
})
export class SmartSpecializationSearchComponent extends BaseNomenclatureSearchComponent<SmartSpecializationDto, SmartSpecializationFilterDto, NomenclatureResource<SmartSpecializationDto, SmartSpecializationFilterDto>> {

  constructor(
    protected override resource: NomenclatureResource<SmartSpecializationDto, SmartSpecializationFilterDto>,
    protected override searchUnsubscriberService: SearchUnsubscriberService,
    public translateService: TranslateService,
  ) {
    super(resource, SmartSpecializationFilterDto, "smartSpecializations", searchUnsubscriberService);
  }
}
