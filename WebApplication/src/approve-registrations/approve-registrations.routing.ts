import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PermissionAuthGuard } from 'src/app/auth-guard/permission.auth-guard';
import { nacidAlias } from 'src/auth/constants/organizational-unit.constants';
import { ApproveRegistrationSearchComponent } from './components/approve-registration-search.component';
import { approvalRegistrationReadPermission } from 'src/auth/constants/permission.constants';

const routes: Routes = [
    {
        path: 'approveRegistrations',
        component: ApproveRegistrationSearchComponent,
        canActivate: [PermissionAuthGuard],
        data: {
            showBreadcrumb: false,
            permission: approvalRegistrationReadPermission,
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
export class ApproveRegistrationRoutingModule { }

