import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { AuthResource } from "src/auth/auth.resource";
import { SsoUserRecoverPasswordDto } from "src/auth/dtos/recover/sso-user-recover-password.dto";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";

@Component({
    selector: 'user-recover-password-confirm',
    templateUrl: './user-recover-password-confirm.component.html'
})
export class UserRecoverPasswordConfirmComponent implements OnInit {

    recoverCode: string;
    userRecoverPasswordDto: SsoUserRecoverPasswordDto = new SsoUserRecoverPasswordDto();

    userRecoverPending = false;

    @ViewChild(NgForm) form: NgForm;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private resource: AuthResource,
        private alertMessageService: AlertMessageService
    ) { }

    recoverPassword() {
        if (this.form.valid) {
            this.userRecoverPending = true;
            this.userRecoverPasswordDto.code = this.recoverCode;
            this.resource.recoverPassword(this.userRecoverPasswordDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.userRecoverPending = false;
                        this.router.navigate(['/']);
                        return throwError(() => err);
                    })
                )
                .subscribe(() => {
                    this.userRecoverPending = false;
                    const alertMessage = new AlertMessageDto('successTexts.userRecovered', 'fa-solid fa-check', null, 'bg-success text-light');
                    this.alertMessageService.show(alertMessage);
                    this.router.navigate(['/']);
                });
        }
    }

    ngOnInit() {
        this.recoverCode = this.activatedRoute.snapshot.queryParams['code'];

        if (!this.recoverCode) {
            this.router.navigate(['/']);
        }
    }
}