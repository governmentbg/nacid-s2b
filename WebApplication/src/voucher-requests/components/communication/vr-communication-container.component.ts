import { Component, Input } from "@angular/core";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { VoucherRequestCommunicationDto } from "src/voucher-requests/dtos/voucher-request-communication.dto";
import { VoucherRequestCommunicationFilterDto } from "src/voucher-requests/filter-dtos/voucher-request-communication-filter.dto";
import { VoucherRequestCommunicationResource } from "src/voucher-requests/resources/voucher-request-communication.resource";
import { VoucherRequestDto } from "src/voucher-requests/dtos/voucher-request.dto";
import { PermissionService } from "src/auth/services/permission.service";
import { CommunicationHubService } from "src/signalR/communication-hub.service";
import { BaseCommunicationContainerComponent } from "src/signalR/notification/components/base/base-communication-container.component";
import { voucherRequestWritePermission } from "src/auth/constants/permission.constants";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'vr-communication-container',
    templateUrl: './vr-communication-container.component.html',
    styleUrls: ['./vr-communication-container.styles.css'],
    providers: [SearchUnsubscriberService, CommunicationHubService]
})
export class VrCommunicationContainerComponent extends BaseCommunicationContainerComponent<VoucherRequestCommunicationDto, VoucherRequestCommunicationFilterDto, VoucherRequestCommunicationResource> {

    hasVoucherRequestWritePermission = this.permissionService.verifyPermission(voucherRequestWritePermission);

    @Input() supplierOffering: SupplierOfferingDto;
    @Input() requestCompanyId: number;

    constructor(
        protected override resource: VoucherRequestCommunicationResource,
        protected override permissionService: PermissionService,
        protected override searchUnsubscriberService: SearchUnsubscriberService,
        protected override communicationHubService: CommunicationHubService,
        protected override configuration: Configuration) {
        super(resource, VoucherRequestCommunicationDto, VoucherRequestCommunicationFilterDto, permissionService, searchUnsubscriberService, communicationHubService, configuration);
    }

    override initialization() {
        this.communicationHubService.connectHub('vrCommunicationHub', this.supplierOffering.id.toString() + this.requestCompanyId.toString());
        this.filter.supplierOfferingId = this.supplierOffering.id;
        this.filter.requestCompanyId = this.requestCompanyId;
        this.currentCommunicationDto.entity = new VoucherRequestDto();
        this.currentCommunicationDto.entity.supplierOfferingId = this.supplierOffering.id;
        this.currentCommunicationDto.entity.supplierOffering = this.supplierOffering;
        this.currentCommunicationDto.entity.requestCompanyId = this.requestCompanyId;

        this.communicationHubService.hubConnectionBuilder.on('SendText', (recievedMessage: VoucherRequestCommunicationDto) => {
            this.autoScrollEnabled = true;
            this.communications.push(recievedMessage);
        });

        return this.getData(false, true);
    }
}