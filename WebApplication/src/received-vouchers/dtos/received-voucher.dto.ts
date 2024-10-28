import { BaseReceivedVoucherDto } from "./base/base-received-voucher.dto";
import { ReceivedVoucherCertificateDto } from "./received-voucher-certificate.dto";
import { ReceivedVoucherFileDto } from "./received-voucher-file.dto";

export class ReceivedVoucherDto extends BaseReceivedVoucherDto {
    file: ReceivedVoucherFileDto;
    certificates: ReceivedVoucherCertificateDto[] = [];
    historiesCount: number;
}