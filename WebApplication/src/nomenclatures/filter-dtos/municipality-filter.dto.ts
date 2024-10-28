import { NomenclatureFilterDto } from "src/nomenclatures/filter-dtos/nomenclature-filter.dto";
import { DistrictDto } from "../dtos/settlements/district.dto";

export class MunicipalityFilterDto extends NomenclatureFilterDto {
  districtId: number;
  district: DistrictDto;
}