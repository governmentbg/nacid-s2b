import { FilterDto } from "src/shared/filter-dtos/filter.dto";

export class ReceivedVoucherCommunicationFilterDto extends FilterDto {
    receivedVoucherId: number;

    fromCreateDate: Date;
    toCreateDate: Date;

    companyId: number;

    supplierId: number;
    offeringId: number;
}