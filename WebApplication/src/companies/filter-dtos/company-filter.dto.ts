import { LawFormDto } from "src/nomenclatures/dtos/law-forms/law-form.dto";
import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { MunicipalityDto } from "src/nomenclatures/dtos/settlements/municipality.dto";
import { SettlementDto } from "src/nomenclatures/dtos/settlements/settlement.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { CompanyType } from "../enums/company-type.enum";

export class CompanyFilterDto extends FilterDto {
    uic: string;

    type: CompanyType;

    name: string;

    lawFormId: number;
    lawForm: LawFormDto;

    districtId: number;
    district: DistrictDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    settlementId: number;
    settlement: SettlementDto;

    email: string;
    phoneNumber: string;
}