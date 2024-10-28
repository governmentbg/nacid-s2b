import { Component } from "@angular/core";
import { UserContextService } from "src/auth/services/user-context.service";
import { ReceivedVoucherCommunicationDto } from "src/received-vouchers/dtos/received-voucher-communication.dto";
import { BaseCommunicationMessageComponent } from "src/signalR/notification/components/base/base-communication-message.component";

@Component({
    selector: 'rv-communication-message',
    templateUrl: './rv-communication-message.component.html',
    styleUrls: ['./rv-communication-message.styles.css']
})
export class RvCommunicationMessageComponent extends BaseCommunicationMessageComponent<ReceivedVoucherCommunicationDto> {

    constructor(public override userContextService: UserContextService) {
        super(userContextService);
    }
}