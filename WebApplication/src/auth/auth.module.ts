import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { AuthResource } from "./auth.resource";
import { AuthRoutingModule } from "./auth.routing";
import { AuthCodeLoginComponent } from "./components/auth-code-login/auth-code-login.component";
import { LoginModalComponent } from "./components/login/login-modal.component";
import { UserContextService } from "./services/user-context.service";
import { PermissionService } from "./services/permission.service";
import { SignUpComponent } from "./components/sign-up/sign-up.component";
import { UserSignUpComponent } from "./components/sign-up/user-sign-up.component";
import { CompanySignUpComponent } from "./components/sign-up/company-sign-up.component";
import { UserActivationComponent } from "./components/activation/user-activation.component";
import { UserRecoverPasswordComponent } from "./components/recover/user-recover-password.component";
import { UserRecoverPasswordConfirmComponent } from "./components/recover/user-recover-password-confirm.component";
import { SupplierSignUpComponent } from "./components/sign-up/supplier-sign-up.component";
import { SignUpSupplierEditComponent } from "./components/sign-up/sign-up-supplier-edit.component";
import { ChangePasswordModalComponent } from "./components/change-password/change-password-modal.component";

@NgModule({
    declarations: [
        AuthCodeLoginComponent,
        LoginModalComponent,
        SignUpComponent,
        SignUpSupplierEditComponent,
        UserSignUpComponent,
        SupplierSignUpComponent,
        CompanySignUpComponent,
        UserActivationComponent,
        UserRecoverPasswordComponent,
        UserRecoverPasswordConfirmComponent,
        ChangePasswordModalComponent
    ],
    imports: [
        AuthRoutingModule,
        SharedModule
    ],
    providers: [
        UserContextService,
        PermissionService,
        AuthResource
    ]
})
export class AuthModule { }
