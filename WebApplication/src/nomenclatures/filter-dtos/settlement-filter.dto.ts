import { NomenclatureFilterDto } from "src/nomenclatures/filter-dtos/nomenclature-filter.dto";
import { DistrictDto } from "../dtos/settlements/district.dto";
import { MunicipalityDto } from "../dtos/settlements/municipality.dto";

export class SettlementFilterDto extends NomenclatureFilterDto {
  districtId: number;
  district: DistrictDto;
  municipalityId: number;
  municipality: MunicipalityDto;
}