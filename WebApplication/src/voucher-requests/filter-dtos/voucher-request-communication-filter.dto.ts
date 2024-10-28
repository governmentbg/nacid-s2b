import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class VoucherRequestCommunicationFilterDto extends FilterDto {
    fromCreateDate: Date;
    toCreateDate: Date;

    requestCompanyId: number;

    supplierId: number;
    supplierOfferingId: number;
}