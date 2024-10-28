import { RouterModule, Routes } from "@angular/router";
import { LogTabsComponent } from "./components/log-tabs.component";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PermissionAuthGuard } from "src/app/auth-guard/permission.auth-guard";
import { nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { systemLogReadPermission } from "src/auth/constants/permission.constants";

const routes: Routes = [
    {
        path: 'logs',
        component: LogTabsComponent,
        canActivate: [PermissionAuthGuard],
        data: {
            title: 'routes.logs',
            permission: systemLogReadPermission,
            unitExternals: [
                [nacidAlias, null]
            ]
        }
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LogRoutingModule { }