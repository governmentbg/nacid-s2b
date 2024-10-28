import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { ReceivedVoucherCertificateFileDto } from "./received-voucher-certificate-file.dto";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";

export class ReceivedVoucherCertificateDto {
    id: number;

    receivedVoucherId: number;

    userId: number;
    username: string;
    userFullname: string;

    file: ReceivedVoucherCertificateFileDto;

    supplierId: number;
    supplier: SupplierDto;

    offeringId: number;
    offering: SupplierOfferingDto;
}