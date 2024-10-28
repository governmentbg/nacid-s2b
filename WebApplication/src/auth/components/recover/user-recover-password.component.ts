import { HttpErrorResponse } from "@angular/common/http";
import { Component, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { AuthResource } from "src/auth/auth.resource";
import { SsoUserEmailDto } from "src/auth/dtos/recover/sso-user-email.dto";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";

@Component({
    selector: 'user-recover-password',
    templateUrl: './user-recover-password.component.html'
})
export class UserRecoverPasswordComponent {

    userEmail = new SsoUserEmailDto();

    recoverUserPending = false;

    @ViewChild(NgForm) form: NgForm;

    constructor(private router: Router,
        private resource: AuthResource,
        private alertMessageService: AlertMessageService) {
    }

    generateCode() {
        if (this.form.valid) {
            this.recoverUserPending = true;
            this.resource.generateRecoverCode(this.userEmail)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.recoverUserPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(() => {
                    this.recoverUserPending = false;
                    const alertMessageUser = new AlertMessageDto('successTexts.userRecoverCodeGenerated', 'fa-solid fa-envelope', null, 'bg-success text-light');
                    this.alertMessageService.show(alertMessageUser);
                    this.router.navigate(['/']);
                });
        }
    }
}