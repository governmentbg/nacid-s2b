import { BaseCommunicationDto } from "src/signalR/notification/dtos/base-communication.dto";
import { VoucherRequestDto } from "./voucher-request.dto";

export class VoucherRequestCommunicationDto extends BaseCommunicationDto {

    entity: VoucherRequestDto;
}