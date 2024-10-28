export class SsoUserValidateSignUpInfoDto {
    exists = false;
    ssoApplicationUnits: SsoUserApplicationUnitInfoDto[] = [];
}

export class SsoUserApplicationUnitInfoDto {
    ssoApplicationName: string;
    organizationalUnitNames: string[] = [];
}