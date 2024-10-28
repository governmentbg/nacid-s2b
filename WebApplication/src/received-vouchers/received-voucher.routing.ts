import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PermissionAuthGuard } from "src/app/auth-guard/permission.auth-guard";
import { receivedVoucherCreatePermission, receivedVoucherReadPermission } from "src/auth/constants/permission.constants";
import { ReceivedVoucherSearchComponent } from "./components/search/received-voucher-search.component";
import { BreadcrumbType } from "src/shared/components/breadcrumb/breadcrumb-type.enum";
import { ReceivedVoucherCreateComponent } from "./components/details/received-voucher-create.component";
import { ReceivedVoucherDetailsComponent } from "./components/details/received-voucher-details.component";
import { HistoryReceivedVoucherDetailsComponent } from "./components/details/history-received-voucher-details.component";

const routes: Routes = [
    {
        path: 'receivedVouchers',
        children: [
            {
                path: '',
                component: ReceivedVoucherSearchComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    showBreadcrumb: false,
                    permission: receivedVoucherReadPermission
                }
            },
            {
                path: 'create',
                component: ReceivedVoucherCreateComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    breadcrumbType: BreadcrumbType.simple,
                    permission: receivedVoucherCreatePermission
                }
            },
            {
                path: ':id',
                component: ReceivedVoucherDetailsComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    breadcrumbType: BreadcrumbType.simple,
                    permission: receivedVoucherReadPermission
                }
            },
            {
                path: 'history',
                component: HistoryReceivedVoucherDetailsComponent,
                canActivate: [PermissionAuthGuard],
                data: {
                    breadcrumbType: BreadcrumbType.simple,
                    permission: receivedVoucherReadPermission
                }
            }
        ]
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReceivedVoucherRoutingModule { }