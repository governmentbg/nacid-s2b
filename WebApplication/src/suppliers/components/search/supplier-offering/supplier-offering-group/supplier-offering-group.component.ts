import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FilterResultGroupDto } from "src/shared/dtos/search/filter-result-group.dto";
import { FilterResultDto } from "src/shared/dtos/search/filter-result.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { smartSpecializations, suppliers } from "src/suppliers/constants/supplier.constants";
import { SupplierOfferingSearchDto } from "src/suppliers/dtos/search/supplier-offering-group/supplier-offering-search.dto";
import { ClearFilterType } from "src/suppliers/enums/clear-filter-type.enum";
import { ClearFilterDto } from "src/suppliers/filter-dtos/clear-filter.dto";
import { SupplierOfferingFilterDto } from "src/suppliers/filter-dtos/supplier-offering-filter.dto";

@Component({
    selector: 'supplier-offering-group',
    templateUrl: './supplier-offering-group.component.html'
})
export class SupplierOfferingGroupComponent {

    suppliers = suppliers;
    smartSpecializations = smartSpecializations;

    @Input() clearFilterDtos: ClearFilterDto[] = [];

    @Input() loadingData = false;
    @Input() filterResultGroup: FilterResultGroupDto<SupplierOfferingSearchDto> = new FilterResultGroupDto<SupplierOfferingSearchDto>();
    @Input() filter = new SupplierOfferingFilterDto();

    @Output() triggerSearch: EventEmitter<void> = new EventEmitter<void>();
    @Output() clearFilterDtosChanged: EventEmitter<ClearFilterDto[]> = new EventEmitter<ClearFilterDto[]>();

    constructor(public pageHandlingService: PageHandlingService) {
    }

    addFilterElement(type: string, item: FilterResultDto) {
        if (type === this.suppliers || type === this.smartSpecializations) {
            if (type === this.suppliers) {
                let supplierClearDto = new ClearFilterDto();
                supplierClearDto.clearType = ClearFilterType.supplierIds;
                supplierClearDto.id = item.id;
                supplierClearDto.value = item.name;
                this.clearFilterDtos.push(supplierClearDto)
                this.clearFilterDtosChanged.emit(this.clearFilterDtos);
                this.filter.supplierIds.push(item.id);
            } else if (type === this.smartSpecializations) {
                let supplierClearDto = new ClearFilterDto();
                supplierClearDto.clearType = ClearFilterType.smartSpecializationRootIds;
                supplierClearDto.id = item.id;
                supplierClearDto.value = `${item.code} ${item.name}`;
                this.clearFilterDtos.push(supplierClearDto)
                this.clearFilterDtosChanged.emit(this.clearFilterDtos);
                this.filter.smartSpecializationRootIds.push(item.id);
            }

            this.triggerSearch.emit();
        }
    }

    removeClearFilterElement(clearFilter: ClearFilterDto) {
        if (clearFilter.clearType === ClearFilterType.name) {
            this.filter.name = null;
            this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== clearFilter.clearType);
            this.clearFilterDtosChanged.emit(this.clearFilterDtos);
        } else if (clearFilter.clearType === ClearFilterType.keywords) {
            this.filter.keywords = null;
            this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== clearFilter.clearType);
            this.clearFilterDtosChanged.emit(this.clearFilterDtos);
        } else if (clearFilter.clearType === ClearFilterType.smartSpecializationRootIds) {
            this.filter.smartSpecializationRootIds = this.filter.smartSpecializationRootIds.filter(id => id !== clearFilter.id);
            this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== clearFilter.clearType || e.id !== clearFilter.id);
            this.filterResultGroup.filterResult[smartSpecializations] = this.filterResultGroup.filterResult[smartSpecializations].filter(e => e.id !== clearFilter.id);
            this.clearFilterDtosChanged.emit(this.clearFilterDtos);

        } else if (clearFilter.clearType === ClearFilterType.supplierIds) {
            this.filter.supplierIds = this.filter.supplierIds.filter(id => id !== clearFilter.id);
            this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== clearFilter.clearType || e.id !== clearFilter.id);
            this.filterResultGroup.filterResult[suppliers] = this.filterResultGroup.filterResult[suppliers].filter(e => e.id !== clearFilter.id);
            this.clearFilterDtosChanged.emit(this.clearFilterDtos);
        }

        this.triggerSearch.emit();
    }

    removeFilterElement(type: string, item: FilterResultDto) {
        if (type === this.suppliers || type === this.smartSpecializations) {
            if (type === this.suppliers) {
                this.filter.supplierIds = this.filter.supplierIds.filter(e => e !== item.id);
                this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== ClearFilterType.supplierIds || e.id !== item.id);
            } else if (type === this.smartSpecializations) {
                this.filter.smartSpecializationRootIds = this.filter.smartSpecializationRootIds.filter(e => e !== item.id);
                this.clearFilterDtos = this.clearFilterDtos.filter(e => e.clearType !== ClearFilterType.smartSpecializationRootIds || e.id !== item.id);
            }

            this.triggerSearch.emit();
        }
    }

    clearFilters() {
        this.filter.supplierIds = [];
        this.filter.smartSpecializationRootIds = [];
        this.filter.name = null;
        this.filter.keywords = null;
        this.clearFilterDtosChanged.emit([]);
        this.triggerSearch.emit();
    }

    calculateTotalPages(totalCount: number, limit: number): number {
        return Math.ceil(totalCount / limit);
    }

}