import { NomenclatureDto } from "../nomenclature.dto";
import { DistrictDto } from "./district.dto";

export class MunicipalityDto extends NomenclatureDto {
    settlementName: string;
    districtId: number;
    district: DistrictDto;
}