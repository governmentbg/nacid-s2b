import { Component, EventEmitter, Input, Output } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { FilterResultDto } from "src/shared/dtos/search/filter-result.dto";

@Component({
    selector: 'filter-select',
    templateUrl: 'filter-select.component.html'
})
export class FilterSelectComponent {

    endIndex = 5;

    @Input() label: string;
    @Input() labelClass: string;
    @Input() class: string;
    @Input() type = "typeName";
    @Input() bgProperty = 'name';
    @Input() enProperty = 'nameAlt';

    filterResult: FilterResultDto[];
    @Input('filterResult')
    set filterResultSetter(filterResult: FilterResultDto[]) {
        this.filterResult = filterResult;
    }

    @Output() addFilterElement: EventEmitter<FilterResultDto> = new EventEmitter<FilterResultDto>();
    @Output() removeFilterElement: EventEmitter<FilterResultDto> = new EventEmitter<FilterResultDto>();

    constructor(public translateService: TranslateService) {
    }

    selectItem(item: FilterResultDto) {
        if (item.isSelected) {
            this.addFilterElement.emit(item);
        } else {
            this.removeFilterElement.emit(item);
        }
    }
}