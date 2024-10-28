import { NgModule } from "@angular/core";
import { CommonReportsTabsComponent } from "./components/tabs/common-reports-tabs.component";
import { SharedModule } from "src/shared/shared.module";
import { ReportRoutingModule } from "./report.routing";
import { OfferingContractReportComponent } from "./components/offering-contracts/offering-contract-report.component";

@NgModule({
    declarations: [
        CommonReportsTabsComponent,
        OfferingContractReportComponent
    ],
    imports: [
        ReportRoutingModule,
        SharedModule
    ]
})
export class ReportModule { }