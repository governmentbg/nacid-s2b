import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { ReceivedVoucherRoutingModule } from "./received-voucher.routing";
import { ReceivedVoucherSearchComponent } from "./components/search/received-voucher-search.component";
import { ReceivedVoucherSearchTrComponent } from "./components/search/received-voucher-search-tr.component";
import { ReceivedVoucherBasicComponent } from "./components/details/basic/received-voucher-basic.component";
import { ReceivedVoucherCreateComponent } from "./components/details/received-voucher-create.component";
import { ReceivedVoucherVerifyStateModalComponent } from "./components/details/modals/received-voucher-verify-state-modal.component";
import { ReceivedVoucherDetailsComponent } from "./components/details/received-voucher-details.component";
import { ReceivedVoucherPermissionService } from "./services/received-voucher-permission.service";
import { ReceivedVoucherResource } from "./resources/received-voucher.resource";
import { ReceivedVoucherHistoryResource } from "./resources/received-voucher-history.resource";
import { HistoryReceivedVoucherDetailsComponent } from "./components/details/history-received-voucher-details.component";
import { ReceivedVoucherCertificateModalComponent } from "./components/details/modals/received-voucher-certificate-modal.component";
import { TerminateReceivedVoucherModal } from "./components/details/modals/received-voucher-terminate-modal.component";
import { ReceivedVoucherCertificateResource } from "./resources/received-voucher-certificate.resource";
import { RvCommunicationMessageComponent } from "./components/communication/rv-communication-message.component";
import { ReceivedVoucherCommunicationResource } from "./resources/received-voucher-communication.resource";
import { RvCommunicationContainerComponent } from "./components/communication/rv-communication-container.component";
import { ReceivedVoucherCountResource } from "./resources/received-voucher-count.resource";

@NgModule({
    declarations: [
        ReceivedVoucherSearchComponent,
        ReceivedVoucherSearchTrComponent,
        ReceivedVoucherBasicComponent,
        ReceivedVoucherCreateComponent,
        ReceivedVoucherDetailsComponent,
        ReceivedVoucherVerifyStateModalComponent,
        ReceivedVoucherCertificateModalComponent,
        RvCommunicationContainerComponent,
        RvCommunicationMessageComponent,
        HistoryReceivedVoucherDetailsComponent,
        TerminateReceivedVoucherModal
    ],
    imports: [
        ReceivedVoucherRoutingModule,
        SharedModule
    ],
    providers: [
        ReceivedVoucherResource,
        ReceivedVoucherHistoryResource,
        ReceivedVoucherCertificateResource,
        ReceivedVoucherCommunicationResource,
        ReceivedVoucherPermissionService,
        ReceivedVoucherCountResource
    ],
    exports: [
    ]
})
export class ReceivedVoucherModule { }