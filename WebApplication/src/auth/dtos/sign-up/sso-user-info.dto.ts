import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { MunicipalityDto } from "src/nomenclatures/dtos/settlements/municipality.dto";
import { SettlementDto } from "src/nomenclatures/dtos/settlements/settlement.dto";

export class SsoUserInfoDto {
    firstName: string;
    middleName: string;
    lastName: string;
    fullName: string;
    districtId: number;
    district: DistrictDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    settlementId: number;
    settlement: SettlementDto;
    address: string;
}