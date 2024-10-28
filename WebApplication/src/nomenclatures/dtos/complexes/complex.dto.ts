import { AreaOfActivity } from "src/nomenclatures/enums/area-of-activity.enum";
import { NomenclatureDto } from "../nomenclature.dto";
import { SettlementDto } from "../settlements/settlement.dto";
import { MunicipalityDto } from "../settlements/municipality.dto";
import { DistrictDto } from "../settlements/district.dto";
import { ComplexOrganizationDto } from "./complex-organization.dto";

export class ComplexDto extends NomenclatureDto {
    lotNumber: number;

    shortName: string;
    shortNameAlt: string;

    coordinatorPosition: string;
    coordinatorPositionAlt: string;
    category: string;
    categoryAlt: string;
    benefits: string;
    benefitsAlt: string;
    scientificTeam: string;
    scientificTeamAlt: string;

    areaOfActivity: AreaOfActivity;

    dateFrom: Date;
    dateTo: Date;
    europeanInfrastructure: string;

    isForeign: boolean;
    settlementId: number;
    settlement: SettlementDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    districtId: number;
    district: DistrictDto;
    foreignSettlement: string;
    foreignSettlementAlt: string;
    address: string;
    addressAlt: string;

    webPageUrl: string;
    postCode: string;
    phone: string;
    fax: string;
    email: string;

    complexOrganizations: ComplexOrganizationDto[] = [];
}