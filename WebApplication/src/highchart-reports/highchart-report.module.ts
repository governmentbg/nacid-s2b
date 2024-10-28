import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { HighchartBgMapComponent } from "./components/highchart-bg-map.component";
import { HighchartReportRoutingModule } from "./highchart-report.routing";
import { BgMapReportResource } from "./resources/bg-map-report.resource";
import { HighchartBgMapMenuComponent } from "./components/highchart-bg-map-menu.component";
import { HighchartBgMapMenuItemComponent } from "./components/highchart-bg-map-menu-item.component";

const components = [
    HighchartBgMapComponent,
    HighchartBgMapMenuComponent,
    HighchartBgMapMenuItemComponent
];

const providers = [
    BgMapReportResource
];

const commonModules = [
    SharedModule,
    HighchartReportRoutingModule
]

@NgModule({
    declarations: components,
    providers: providers,
    imports: commonModules,
    exports: [...commonModules, ...components]
})
export class HighchartReportModule { }