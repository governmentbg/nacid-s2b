import { LawFormDto } from "src/nomenclatures/dtos/law-forms/law-form.dto";
import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { MunicipalityDto } from "src/nomenclatures/dtos/settlements/municipality.dto";
import { SettlementDto } from "src/nomenclatures/dtos/settlements/settlement.dto";
import { CompanyType } from "../enums/company-type.enum";

export class CompanyDto {
    id: number;

    type: CompanyType;

    uic: string;

    lawFormId: number;
    lawForm: LawFormDto;

    name: string;
    nameAlt: string;

    shortName: string;
    shortNameAlt: string;

    settlementId: number;
    settlement: SettlementDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    districtId: number;
    district: DistrictDto;

    address: string;
    addressAlt: string;
    email: string;
    phoneNumber: string;

    isActive: boolean;
    isRegistryAgency: boolean;
}