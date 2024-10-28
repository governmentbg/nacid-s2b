import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CommunicationResource } from "src/signalR/notification/resources/communication.resource";
import { ReceivedVoucherCommunicationDto } from "../dtos/received-voucher-communication.dto";
import { ReceivedVoucherCommunicationFilterDto } from "../filter-dtos/received-voucher-communication-filter.dto";

@Injectable()
export class ReceivedVoucherCommunicationResource extends CommunicationResource<ReceivedVoucherCommunicationDto, ReceivedVoucherCommunicationFilterDto> {

    constructor(
        protected override http: HttpClient
    ) {
        super(http);
        this.init('receivedVoucherCommunications');
    }
}