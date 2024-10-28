import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { VoucherRequestSearchComponent } from "./components/search/voucher-request-search.component";
import { PermissionAuthGuard } from "src/app/auth-guard/permission.auth-guard";
import { voucherRequestReadPermission } from "src/auth/constants/permission.constants";
import { BreadcrumbType } from "src/shared/components/breadcrumb/breadcrumb-type.enum";
import { VoucherRequestDetailsComponent } from "./components/voucher-request-details.component";

const routes: Routes = [
    {
        path: 'voucherRequests',
        children: [
            {
                path: '',
                component: VoucherRequestSearchComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    showBreadcrumb: false,
                    permission: voucherRequestReadPermission
                }
            },
            {
                path: ':id',
                component: VoucherRequestDetailsComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    breadcrumbType: BreadcrumbType.simple,
                    permission: voucherRequestReadPermission
                }
            }
        ]
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class VoucherRequestRoutingModule { }