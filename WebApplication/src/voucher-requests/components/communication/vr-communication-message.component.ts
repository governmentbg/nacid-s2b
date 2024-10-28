import { Component } from "@angular/core";
import { UserContextService } from "src/auth/services/user-context.service";
import { BaseCommunicationMessageComponent } from "src/signalR/notification/components/base/base-communication-message.component";
import { VoucherRequestCommunicationDto } from "src/voucher-requests/dtos/voucher-request-communication.dto";

@Component({
    selector: 'vr-communication-message',
    templateUrl: './vr-communication-message.component.html',
    styleUrls: ['./vr-communication-message.styles.css']
})
export class VrCommunicationMessageComponent extends BaseCommunicationMessageComponent<VoucherRequestCommunicationDto> {

    constructor(public override userContextService: UserContextService) {
        super(userContextService);
    }
}