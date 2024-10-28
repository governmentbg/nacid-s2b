import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { LogRoutingModule } from "./log.routing";
import { LogTabsComponent } from "./components/log-tabs.component";
import { ActionLogSearchComponent } from "./components/action-log-search.component";
import { ErrorLogSearchComponent } from "./components/error-log-search.component";
import { ErrorLogModalComponent } from "./components/modals/error-log-modal.component";
import { ActionLogModalComponent } from "./components/modals/action-log-modal.component";

@NgModule({
    declarations: [
        LogTabsComponent,
        ActionLogSearchComponent,
        ActionLogModalComponent,
        ErrorLogSearchComponent,
        ErrorLogModalComponent
    ],
    imports: [
        LogRoutingModule,
        SharedModule
    ]
})
export class LogModule { }
