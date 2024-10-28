import { NomenclatureHierarchyDto } from "src/nomenclatures/dtos/nomenclature-hierarchy.dto";
import { OrganizationType } from "../../enums/organization-type.enum";
import { SettlementDto } from "src/nomenclatures/dtos/settlements/settlement.dto";
import { MunicipalityDto } from "src/nomenclatures/dtos/settlements/municipality.dto";
import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { OwnershipType } from "src/nomenclatures/enums/ownership-type.enum";

export class InstitutionDto extends NomenclatureHierarchyDto<InstitutionDto> {
    lotNumber: number;

    uic: string;
    shortName: string;
    shortNameAlt: string;

    organizationType: OrganizationType;
    ownershipType: OwnershipType;

    settlementId: number;
    settlement: SettlementDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    districtId: number;
    district: DistrictDto;
    address: string;
    addressAlt: string;
    webPageUrl: string;

    children: InstitutionDto[];
}