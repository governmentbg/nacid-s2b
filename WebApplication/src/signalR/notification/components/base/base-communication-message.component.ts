import { Directive, Input, OnInit } from "@angular/core";
import { BaseCommunicationDto } from "../../dtos/base-communication.dto";
import { UserContextService } from "src/auth/services/user-context.service";

@Directive()
export class BaseCommunicationMessageComponent<T extends BaseCommunicationDto> implements OnInit {

    showDate = false;

    @Input() communicationMessage: T;
    @Input() previousCommunicationMessage: T;

    constructor(public userContextService: UserContextService) {
    }

    ngOnInit() {
        const messageDate = new Date(this.communicationMessage.createDate).toDateString();
        this.showDate = this.previousCommunicationMessage != null
            ? (messageDate !== new Date(this.previousCommunicationMessage.createDate).toDateString() || this.communicationMessage.fromUserId !== this.previousCommunicationMessage.fromUserId)
            : true;
    }
}