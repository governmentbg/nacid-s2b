import { RouterModule, Routes } from "@angular/router";
import { EAuthResponseComponent } from "./components/e-auth-response.component";
import { BreadcrumbType } from "src/shared/components/breadcrumb/breadcrumb-type.enum";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

const routes: Routes = [
    {
        path: "eAuthResponse",
        component: EAuthResponseComponent,
        data: {
            breadcrumbType: BreadcrumbType.simple
        }
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EAuthRoutingModule { }