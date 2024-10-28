import { OrganizationalUnitContext } from "./organizational-unit-context.dto";

export class UserContext {
    clientId: string;
    userId: number;
    userName: string;
    fullName: string;
    phoneNumber: string;
    organizationalUnits: OrganizationalUnitContext[] = [];

    // If company user
    companyId: number;
}