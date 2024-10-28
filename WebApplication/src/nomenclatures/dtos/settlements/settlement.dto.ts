import { NomenclatureDto } from "../nomenclature.dto";
import { DistrictDto } from "./district.dto";
import { MunicipalityDto } from "./municipality.dto";

export class SettlementDto extends NomenclatureDto {
    districtId: number;
    district: DistrictDto;
    municipalityId: number;
    municipality: MunicipalityDto;
}