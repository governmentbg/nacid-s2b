import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ClearFilterDto } from "src/suppliers/filter-dtos/clear-filter.dto";

@Component({
    selector: 'filter-summary',
    templateUrl: './filter-summary.component.html',
    styleUrls: ['./filter-summary.component.css']
})
export class FilterSummaryComponent {
    @Input() clearFilterDtos: ClearFilterDto[] = [];

    @Output() removeClearFilter = new EventEmitter<ClearFilterDto>();
    @Output() clearFilters = new EventEmitter<void>();

    onRemoveClearFilter(filter: ClearFilterDto) {
        this.removeClearFilter.emit(filter);
    }

    onClearFilters() {
        this.clearFilters.emit();
    }
}
