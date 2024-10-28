import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthCodeLoginComponent } from './components/auth-code-login/auth-code-login.component';
import { LogoutAuthGuard } from 'src/app/auth-guard/logout.auth-guard';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { UserActivationComponent } from './components/activation/user-activation.component';
import { SignUpType } from './enums/sign-up-type.enum';
import { UserRecoverPasswordComponent } from './components/recover/user-recover-password.component';
import { UserRecoverPasswordConfirmComponent } from './components/recover/user-recover-password-confirm.component';
import { PermissionAuthGuard } from 'src/app/auth-guard/permission.auth-guard';
import { approvalRegistrationWritePermission } from './constants/permission.constants';
import { nacidAlias } from './constants/organizational-unit.constants';
import { BreadcrumbType } from 'src/shared/components/breadcrumb/breadcrumb-type.enum';
import { SignUpSupplierEditComponent } from './components/sign-up/sign-up-supplier-edit.component';

const routes: Routes = [
    {
        path: 'login',
        component: AuthCodeLoginComponent,
        canActivate: [LogoutAuthGuard]
    },
    {
        path: 'signUpSupplier',
        component: SignUpComponent,
        canActivate: [LogoutAuthGuard],
        data: {
            signUpType: SignUpType.supplier,
            breadcrumbType: BreadcrumbType.simple,
            showBackButton: false
        }
    },
    {
        path: 'signUpSupplier/:id',
        component: SignUpSupplierEditComponent,
        canActivate: [PermissionAuthGuard],
        data: {
            breadcrumbType: BreadcrumbType.simple,
            permission: approvalRegistrationWritePermission,
            unitExternals: [
                [nacidAlias, null]
            ]
        }
    },
    {
        path: 'signUp',
        component: SignUpComponent,
        canActivate: [LogoutAuthGuard],
        data: {
            signUpType: SignUpType.company,
            breadcrumbType: BreadcrumbType.simple
        }
    },
    {
        path: 'userActivation',
        component: UserActivationComponent
    },
    {
        path: 'userRecover',
        component: UserRecoverPasswordComponent,
        canActivate: [LogoutAuthGuard]
    },
    {
        path: 'userRecover/confirm',
        component: UserRecoverPasswordConfirmComponent,
        canActivate: [LogoutAuthGuard]
    }
];

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuthRoutingModule { }

