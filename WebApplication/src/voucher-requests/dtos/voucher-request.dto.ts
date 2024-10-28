import { CompanyDto } from "src/companies/dtos/company.dto";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { VoucherRequestState } from "../enums/voucher-request-state.enum";

export class VoucherRequestDto {
    id: number;

    code: string;

    state: VoucherRequestState;

    createDate: Date;

    requestUserId: number;
    requestCompanyId: number;
    requestCompany: CompanyDto;

    supplierOfferingId: number;
    supplierOffering: SupplierOfferingDto;
    declineReason: string;
}