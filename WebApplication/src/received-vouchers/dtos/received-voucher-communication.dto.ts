import { BaseCommunicationDto } from "src/signalR/notification/dtos/base-communication.dto";
import { ReceivedVoucherDto } from "./received-voucher.dto";

export class ReceivedVoucherCommunicationDto extends BaseCommunicationDto {

    entity: ReceivedVoucherDto;
}