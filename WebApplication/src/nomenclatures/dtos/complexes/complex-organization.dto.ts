import { OrganizationTypeEnum } from "src/nomenclatures/enums/organization-type-enum.enum";

export class ComplexOrganizationDto {
    id: number;
    complexId: number;

    organizationTypeEnum: OrganizationTypeEnum;

    financingOrganizationLotId: number;
    financingOrganizationName: string;
    financingOrganizationNameAlt: string;
    financingOrganizationShortName: string;
    financingOrganizationShortNameAlt: string;

    organizationLotId: number;
    organizationName: string;
    organizationNameAlt: string;
    organizationShortName: string;
    organizationShortNameAlt: string;

    nameRND: string;
    nameRNDAlt: string;
}