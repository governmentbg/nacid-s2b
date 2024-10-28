import { VoucherRequestState } from "../enums/voucher-request-state.enum";

export class VoucherRequestStateDto {
    requestCompanyId: number;
    supplierOfferingId: number;
    state: VoucherRequestState;
}