import { HttpErrorResponse } from "@angular/common/http";
import { Component, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, NavigationEnd, Router } from "@angular/router";
import { RecaptchaComponent } from "ng-recaptcha";
import { catchError, filter, throwError } from "rxjs";
import { AuthResource } from "src/auth/auth.resource";
import { SignUpDeclarationDto } from "src/auth/dtos/sign-up/sign-up-declaration.dto";
import { SignUpType } from "src/auth/enums/sign-up-type.enum";
import { UserAuthorizationState } from "src/auth/enums/user-authorization-state.enum";
import { UserContextService } from "src/auth/services/user-context.service";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";

@Component({
    selector: 'sign-up',
    templateUrl: './sign-up.component.html'
})
export class SignUpComponent {

    signUpDeclarationDto = new SignUpDeclarationDto();
    registrationSuccessMessage: boolean = false;
    registrationSupplierSuccessMessage: boolean = false;
    signUpPending = false;
    authorizationState = UserAuthorizationState;
    signUpType = SignUpType;

    @ViewChild(NgForm) form: NgForm;
    @ViewChild('recaptcha') recaptchaElement: RecaptchaComponent;

    constructor(
        public userContextService: UserContextService,
        private resource: AuthResource,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private pageHandlingService: PageHandlingService) {
        this.router.events
            .pipe(filter(event => event instanceof NavigationEnd))
            .subscribe(() => {
                this.signUpDeclarationDto.signUp.type = this.activatedRoute.root.firstChild.snapshot.data['signUpType'];
            });
    }

    signUp() {
        if (this.form.valid) {
            this.signUpPending = true;
            this.resource
                .signUp(this.signUpDeclarationDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.signUpPending = false;
                        this.recaptchaElement.reset();
                        this.pageHandlingService.scrollToTop();
                        return throwError(() => err);
                    })
                )
                .subscribe((supplierDto: SupplierDto) => {
                    this.signUpPending = false;
                    this.pageHandlingService.scrollToTop();
                    if (this.signUpDeclarationDto.signUp.type == SignUpType.supplier) {
                        this.registrationSupplierSuccessMessage = true;
                    } else if (this.signUpDeclarationDto.signUp.type == SignUpType.company) {
                        this.registrationSuccessMessage = true;
                    }
                });
        }
    }
}

