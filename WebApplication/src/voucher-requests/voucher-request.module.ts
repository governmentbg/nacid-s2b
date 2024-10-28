import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { VoucherRequestResource } from "./resources/voucher-request.resource";
import { VoucherRequestRoutingModule } from "./voucher-request.routing";
import { VoucherRequestCommunicationResource } from "./resources/voucher-request-communication.resource";
import { VoucherRequestSearchComponent } from "./components/search/voucher-request-search.component";
import { VoucherRequestSearchTrComponent } from "./components/search/voucher-request-search-tr.component";
import { VoucherRequestDetailsComponent } from "./components/voucher-request-details.component";
import { VrCommunicationContainerComponent } from "./components/communication/vr-communication-container.component";
import { VrCommunicationMessageComponent } from "./components/communication/vr-communication-message.component";
import { VoucherRequestDeclineCodeComponent } from "./components/modals/voucher-request-decline-code-modal.component";

@NgModule({
    declarations: [
        VoucherRequestSearchComponent,
        VoucherRequestSearchTrComponent,
        VoucherRequestDetailsComponent,
        VrCommunicationContainerComponent,
        VrCommunicationMessageComponent,
        VoucherRequestDeclineCodeComponent
    ],
    imports: [
        VoucherRequestRoutingModule,
        SharedModule
    ],
    providers: [
        VoucherRequestResource,
        VoucherRequestCommunicationResource
    ],
    exports: [
        VrCommunicationContainerComponent
    ]
})
export class VoucherRequestModule { }