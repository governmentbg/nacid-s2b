import { BaseReceivedVoucherDto } from "./base/base-received-voucher.dto";
import { ReceivedVoucherHistoryFileDto } from "./received-voucher-history-file.dto";

export class ReceivedVoucherHistoryDto extends BaseReceivedVoucherDto {
    receivedVoucherId: number;
    file: ReceivedVoucherHistoryFileDto;
}