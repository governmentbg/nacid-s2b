import { Component } from "@angular/core";
import { Configuration } from "src/app/configuration/configuration";
import { receivedVoucherWritePermission } from "src/auth/constants/permission.constants";
import { PermissionService } from "src/auth/services/permission.service";
import { ReceivedVoucherCommunicationDto } from "src/received-vouchers/dtos/received-voucher-communication.dto";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherCommunicationFilterDto } from "src/received-vouchers/filter-dtos/received-voucher-communication-filter.dto";
import { ReceivedVoucherCommunicationResource } from "src/received-vouchers/resources/received-voucher-communication.resource";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { CommunicationHubService } from "src/signalR/communication-hub.service";
import { BaseCommunicationContainerComponent } from "src/signalR/notification/components/base/base-communication-container.component";

@Component({
    selector: 'rv-communication-container',
    templateUrl: './rv-communication-container.component.html',
    styleUrls: ['./rv-communication-container.styles.css'],
    providers: [SearchUnsubscriberService, CommunicationHubService]
})
export class RvCommunicationContainerComponent extends BaseCommunicationContainerComponent<ReceivedVoucherCommunicationDto, ReceivedVoucherCommunicationFilterDto, ReceivedVoucherCommunicationResource> {

    hasReceivedVoucherWritePermission = this.permissionService.verifyPermission(receivedVoucherWritePermission);

    constructor(
        protected override resource: ReceivedVoucherCommunicationResource,
        protected override permissionService: PermissionService,
        protected override searchUnsubscriberService: SearchUnsubscriberService,
        protected override communicationHubService: CommunicationHubService,
        protected override configuration: Configuration) {
        super(resource, ReceivedVoucherCommunicationDto, ReceivedVoucherCommunicationFilterDto, permissionService, searchUnsubscriberService, communicationHubService, configuration);
    }

    override initialization() {
        this.communicationHubService.connectHub('rvCommunicationHub', this.entityId.toString());
        this.filter.receivedVoucherId = this.entityId;
        this.currentCommunicationDto.entity = new ReceivedVoucherDto();
        this.currentCommunicationDto.entityId = this.entityId;

        this.communicationHubService.hubConnectionBuilder.on('SendText', (recievedMessage: ReceivedVoucherCommunicationDto) => {
            this.autoScrollEnabled = true;
            this.communications.push(recievedMessage);
        });

        return this.getData(false, true);
    }
}