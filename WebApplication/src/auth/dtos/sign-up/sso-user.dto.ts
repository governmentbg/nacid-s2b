import { SsoUserInfoDto } from "./sso-user-info.dto";

export class SsoUserDto {
    userName: string;
    confirmUserName: string;
    phoneNumber: string;

    userInfo: SsoUserInfoDto = new SsoUserInfoDto();
}