import { VoucherRequestState } from "../enums/voucher-request-state.enum";

export class ChangedStateDto {
    state: VoucherRequestState;
    code: string;
}