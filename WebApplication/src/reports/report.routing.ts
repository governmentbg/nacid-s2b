import { RouterModule, Routes } from "@angular/router";
import { CommonReportsTabsComponent } from "./components/tabs/common-reports-tabs.component";
import { PermissionAuthGuard } from "src/app/auth-guard/permission.auth-guard";
import { BreadcrumbType } from "src/shared/components/breadcrumb/breadcrumb-type.enum";
import { commonReportsReadPermission } from "src/auth/constants/permission.constants";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

const routes: Routes = [
    {
        path: 'report',
        children: [
            {
                path: 'common',
                component: CommonReportsTabsComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    breadcrumbType: BreadcrumbType.simple,
                    permission: commonReportsReadPermission
                }
            }
        ]
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReportRoutingModule { }