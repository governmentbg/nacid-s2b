import { RouterModule, Routes } from "@angular/router";
import { HighchartBgMapComponent } from "./components/highchart-bg-map.component";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BreadcrumbType } from "src/shared/components/breadcrumb/breadcrumb-type.enum";

const routes: Routes = [
    {
        path: 'bgMap',
        component: HighchartBgMapComponent,
        data: {
            breadcrumbType: BreadcrumbType.simple,
            title: 'routes.bgMap',
        }
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HighchartReportRoutingModule { }