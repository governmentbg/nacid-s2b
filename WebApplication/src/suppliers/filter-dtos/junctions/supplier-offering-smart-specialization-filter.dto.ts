import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class SupplierOfferingSmartSpecializationFilterDto extends FilterDto {
    uic: string;
    name: string;
    districtId: number;
    district: DistrictDto;

    override limit = 10;
}