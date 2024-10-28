import { ApproveRegistrationFileDto } from "../approve-registration-file.dto";
import { BaseApproveRegistrationDto } from "../base/base-approve-registration.dto";
import { ApproveRegistrationHistorySearchDto } from "./approve-registration-history-search.dto";

export class ApproveRegistrationSearchDto extends BaseApproveRegistrationDto {
  file: ApproveRegistrationFileDto;
  approveRegistrationHistories: ApproveRegistrationHistorySearchDto[] = [];
}

