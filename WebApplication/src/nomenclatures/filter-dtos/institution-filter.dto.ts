import { NomenclatureHierarchyFilterDto } from "src/nomenclatures/filter-dtos/nomenclature-hierarchy-filter.dto";
import { InstitutionDto } from "../dtos/institutions/institution.dto";
import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { MunicipalityDto } from "src/nomenclatures/dtos/settlements/municipality.dto";
import { SettlementDto } from "src/nomenclatures/dtos/settlements/settlement.dto";
import { OrganizationType } from "../enums/organization-type.enum";
import { OwnershipType } from "../enums/ownership-type.enum";

export class InstitutionFilterDto extends NomenclatureHierarchyFilterDto<InstitutionDto> {
    uic: string;

    districtId: number;
    district: DistrictDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    settlementId: number;
    settlement: SettlementDto;

    organizationType: OrganizationType;
    ownershipType: OwnershipType;
}