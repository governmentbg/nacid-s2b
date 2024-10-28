import { SmartSpecializationDto } from "../dtos/smart-specializations/smart-specialization.dto";
import { NomenclatureFilterDto } from "./nomenclature-filter.dto";

export class DistrictFilterDto extends NomenclatureFilterDto {
    smartSpecializationId: number;
    smartSpecialization: SmartSpecializationDto;
}