import { CompanyDto } from "src/companies/dtos/company.dto";
import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { ReceivedVoucherState } from "../enums/received-voucher-state.enum";

export class ReceivedVoucherFilterDto extends FilterDto {
    fromContractDate: Date;
    toContractDate: Date;
    contractNumber: string;

    state: ReceivedVoucherState;

    companyId: number;
    company: CompanyDto;

    supplierId: number;
    supplier: SupplierDto;

    offeringId: number;
    offering: SupplierOfferingDto;
}