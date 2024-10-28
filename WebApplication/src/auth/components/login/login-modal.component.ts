import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, ViewChild } from "@angular/core";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { RecaptchaComponent } from 'ng-recaptcha';
import { catchError, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { AuthResource } from "src/auth/auth.resource";
import { Auth_InvalidRecaptcha, Auth_WrongUserNameOrPassword } from "src/auth/constants/auth.constants";
import { LoginDto } from "src/auth/dtos/login/login.dto";
import { TokenResponseDto } from "src/auth/dtos/token/token-response.dto";
import { EAuthSendRequestModalComponent } from "src/e-auth/components/e-auth-send-request-modal.component";
import { DomainErrorMessageDto } from "src/shared/components/alert-message/models/domain-error-message.dto";

@Component({
    selector: 'login-modal',
    templateUrl: './login-modal.component.html'
})
export class LoginModalComponent {

    loginDto = new LoginDto();
    loginPending = false;
    failedLoginCount = 0;

    @ViewChild('recaptcha') recaptchaElement: RecaptchaComponent;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.loginPending) {
            this.close();
        }
    }

    constructor(
        public configuration: Configuration,
        private activeModal: NgbActiveModal,
        private authResource: AuthResource,
        private modalService: NgbModal
    ) {
    }

    login() {
        this.loginPending = true;
        return this.authResource.login(this.loginDto)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.handleFailedLogin(err);
                    return throwError(() => new Error('Invalid login.'));
                })
            )
            .subscribe((tokenRes: TokenResponseDto) => {
                this.activeModal.close(tokenRes);
            });
    }

    close() {
        this.activeModal.close(null);
    }

    loginEAuth() {
        this.modalService.open(EAuthSendRequestModalComponent, { size: 'lg', keyboard: false, backdrop: 'static' });
    }

    private handleFailedLogin(err: HttpErrorResponse) {
        this.loginPending = false;

        if (err.status === 422) {
            let domainError = err.error as DomainErrorMessageDto;

            if ((domainError.errorCode === Auth_WrongUserNameOrPassword || domainError.errorCode === Auth_InvalidRecaptcha)
                && domainError.errorCount > 0) {
                this.failedLoginCount = domainError.errorCount;

                if (this.recaptchaElement) {
                    this.recaptchaElement.reset();
                }
            }
        }
    }
}