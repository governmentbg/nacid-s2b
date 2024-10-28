import { Component } from '@angular/core';
import { BaseNomenclatureSearchComponent } from '../base/base-nomenclature-search.component';
import { LawFormDto } from 'src/nomenclatures/dtos/law-forms/law-form.dto';
import { NomenclatureFilterDto } from 'src/nomenclatures/filter-dtos/nomenclature-filter.dto';
import { NomenclatureResource } from 'src/nomenclatures/nomenclature.resource';
import { SearchUnsubscriberService } from 'src/shared/services/search-unsubscriber/search-unsubscriber.service';

@Component({
  selector: 'law-form-search',
  templateUrl: './law-form-search.component.html',
  providers: [
    NomenclatureResource,
    SearchUnsubscriberService
  ]
})
export class LawFormSearchComponent extends BaseNomenclatureSearchComponent<LawFormDto, NomenclatureFilterDto, NomenclatureResource<LawFormDto, NomenclatureFilterDto>> {

  constructor(
    protected override resource: NomenclatureResource<LawFormDto, NomenclatureFilterDto>,
    protected override searchUnsubscriberService: SearchUnsubscriberService,
  ) {
    super(resource, NomenclatureFilterDto, "lawForms", searchUnsubscriberService);
  }
}
