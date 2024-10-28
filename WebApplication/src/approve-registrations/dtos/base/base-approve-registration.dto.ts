import { ApproveRegistrationState } from "src/approve-registrations/enums/approve-registration-state.enum";
import { SignUpDto } from "src/auth/dtos/sign-up/sign-up.dto";

export class BaseApproveRegistrationDto {
    id: number;
    createDate: Date;
    finishDate: Date;
    administratedUserId: number;
    administratedUsername: string;
    state: ApproveRegistrationState;
    declinedNote: string;
    signUpDto: SignUpDto;
    supplierId: number;
}