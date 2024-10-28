export class OrganizationalUnitContext {
    organizationalUnitId: number;
    alias: string;
    externalId: number;
    name: string;
    shortName: string;
    permissions: string[] = [];

    // Is set from NacidSc only if user is for institution or complex
    supplierId: number;

    // Is set from NacidSc server not from SSO
    isActive = true;
}