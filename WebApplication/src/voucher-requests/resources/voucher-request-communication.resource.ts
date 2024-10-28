import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { VoucherRequestCommunicationFilterDto } from "../filter-dtos/voucher-request-communication-filter.dto";
import { VoucherRequestCommunicationDto } from "../dtos/voucher-request-communication.dto";
import { CommunicationResource } from "src/signalR/notification/resources/communication.resource";

@Injectable()
export class VoucherRequestCommunicationResource extends CommunicationResource<VoucherRequestCommunicationDto, VoucherRequestCommunicationFilterDto> {

    constructor(
        protected override http: HttpClient
    ) {
        super(http);
        this.init('voucherRequestCommunications');
    }
}