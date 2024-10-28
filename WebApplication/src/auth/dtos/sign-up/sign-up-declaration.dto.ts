import { ApproveRegistrationFileDto } from "src/approve-registrations/dtos/approve-registration-file.dto";
import { SignUpDto } from "./sign-up.dto";

export class SignUpDeclarationDto {
    file: ApproveRegistrationFileDto;
    signUp: SignUpDto = new SignUpDto();
}