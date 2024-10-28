import { NomenclatureHierarchyFilterDto } from "src/nomenclatures/filter-dtos/nomenclature-hierarchy-filter.dto";
import { SmartSpecializationDto } from "../dtos/smart-specializations/smart-specialization.dto";

export class SmartSpecializationFilterDto extends NomenclatureHierarchyFilterDto<SmartSpecializationDto> {

    constructor() {
        super();
        this.level = 2;
    }
}