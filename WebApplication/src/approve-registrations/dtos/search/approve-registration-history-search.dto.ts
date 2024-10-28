import { ApproveRegistrationHistoryFileDto } from "../approve-registration-history-file.dto";
import { BaseApproveRegistrationDto } from "../base/base-approve-registration.dto";

export class ApproveRegistrationHistorySearchDto extends BaseApproveRegistrationDto {
    file: ApproveRegistrationHistoryFileDto;
    approveRegistrationId: number;
}