import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthResource } from 'src/auth/auth.resource';
import { SsoUserActivationDto } from 'src/auth/dtos/activation/sso-user-activation.dto';
import { UserAuthorizationState } from 'src/auth/enums/user-authorization-state.enum';
import { UserContextService } from 'src/auth/services/user-context.service';
import { AlertMessageDto } from 'src/shared/components/alert-message/models/alert-message.dto';
import { AlertMessageService } from 'src/shared/components/alert-message/services/alert-message.service';

@Component({
    selector: 'user-activation',
    templateUrl: './user-activation.component.html'
})
export class UserActivationComponent implements OnInit {

    activationCode: string;
    userActivationDto: SsoUserActivationDto = new SsoUserActivationDto();

    activationPending = false;
    authorizationState = UserAuthorizationState;

    @ViewChild(NgForm) form: NgForm;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private resource: AuthResource,
        private alertMessageService: AlertMessageService,
        public userContextService: UserContextService
    ) { }

    activateUser() {
        if (this.form.valid) {
            this.userActivationDto.code = this.activationCode;
            this.activationPending = true;
            this.resource
                .activateUser(this.userActivationDto).pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.activationPending = false;
                        this.router.navigate(['/']);
                        return throwError(() => err);
                    })
                )
                .subscribe(() => {
                    this.activationPending = false;
                    const alertMessage = new AlertMessageDto('successTexts.userActivated', 'fa-solid fa-check', null, 'bg-success text-light');
                    this.alertMessageService.show(alertMessage);
                    this.router.navigate(['/']);
                });
        }
    }

    ngOnInit() {
        if (this.userContextService.authorizationState === this.authorizationState.login || this.userContextService.authorizationState === this.authorizationState.eAuthLogin) {
            this.userContextService.logout(false);
        }

        this.activationCode = this.activatedRoute.snapshot.queryParams['code'];

        if (!this.activationCode) {
            this.router.navigate(['/']);
        }
    }
}
