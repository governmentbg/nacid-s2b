import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { VoucherRequestState } from "../enums/voucher-request-state.enum";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { CompanyDto } from "src/companies/dtos/company.dto";

export class VoucherRequestFilterDto extends FilterDto {
    state: VoucherRequestState;

    fromCreateDate: Date;
    toCreateDate: Date;

    requestCompany: CompanyDto
    requestCompanyId: number;

    supplier: SupplierDto
    supplierId: number;
    supplierOfferingId: number;
}