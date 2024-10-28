import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { DapperFilterDto } from "./base/dapper-filter.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { ComplexDto } from "src/nomenclatures/dtos/complexes/complex.dto";
import { CompanyDto } from "src/companies/dtos/company.dto";

export class OfferingContractReportFilterDto extends DapperFilterDto {
    state: ReceivedVoucherState;

    fromContractDate: Date;
    toContractDate: Date;

    supplierType: SupplierType;
    rootInstitutionId: number;
    rootInstitution: InstitutionDto;
    institutionId: number;
    institution: InstitutionDto;
    complexId: number;
    complex: ComplexDto;

    companyId: number;
    company: CompanyDto;
}