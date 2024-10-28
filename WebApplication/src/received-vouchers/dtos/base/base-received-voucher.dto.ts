import { CompanyDto } from "src/companies/dtos/company.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";

export class BaseReceivedVoucherDto {
    id: number;

    contractDate: Date;
    contractNumber: string;

    state: ReceivedVoucherState;

    companyUserId: number;
    companyId: number;
    company: CompanyDto;

    supplierId: number;
    supplier: SupplierDto;
    offeringId: number;
    offering: SupplierOfferingDto;
    offeringAdditionalPayment: boolean;
    receivedOffering: string;
    offeringClarifications: string;

    secondSupplierId: number;
    secondSupplier: SupplierDto;
    secondOfferingId: number;
    secondOffering: SupplierOfferingDto;
    secondOfferingAdditionalPayment: boolean;
    secondReceivedOffering: string;
    secondOfferingClarifications: string;
}