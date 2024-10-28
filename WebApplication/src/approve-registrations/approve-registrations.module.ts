import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { ApproveRegistrationsResource } from "./approve-registrations.resource";
import { ApproveRegistrationSearchComponent } from "./components/approve-registration-search.component";
import { ApproveRegistrationRoutingModule } from "./approve-registrations.routing";
import { ApproveRegistrationModalComponent } from "./components/modals/approve-registration-modal.component";
import { DeclineRegistrationModalComponent } from "./components/modals/decline-registration-modal.component";
import { HistoryRegistrationDetailsComponent } from "./components/history-registration-details.component";

@NgModule({
    declarations: [
        ApproveRegistrationSearchComponent,
        ApproveRegistrationModalComponent,
        DeclineRegistrationModalComponent,
        HistoryRegistrationDetailsComponent
    ],
    imports: [
        SharedModule,
        ApproveRegistrationRoutingModule,
    ],
    providers: [
        ApproveRegistrationsResource
    ]
})
export class ApproveRegistrationsModule { }
