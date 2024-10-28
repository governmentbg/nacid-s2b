import { HttpErrorResponse } from "@angular/common/http";
import { Component, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { ApproveRegistrationsResource } from "src/approve-registrations/approve-registrations.resource";
import { ApproveRegistrationSearchDto } from "src/approve-registrations/dtos/search/approve-registration-search.dto";
import { ApproveRegistrationState } from "src/approve-registrations/enums/approve-registration-state.enum";
import { nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { approvalRegistrationWritePermission } from "src/auth/constants/permission.constants";
import { SignUpEditType } from "src/auth/enums/sign-up-edit-type.enum";
import { PermissionService } from "src/auth/services/permission.service";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";

@Component({
    selector: 'sign-up-supplier-edit',
    templateUrl: './sign-up-supplier-edit.component.html'
})
export class SignUpSupplierEditComponent {

    approveRegistrationDto = new ApproveRegistrationSearchDto();
    originalModel = new ApproveRegistrationSearchDto();

    signUpEditType = SignUpEditType;
    state = ApproveRegistrationState;

    editState = SignUpEditType.disabled;
    loadingData = false;
    signUpPending = false;
    hasApprovalRegistrationWritePermission = false;

    @ViewChild(NgForm) form: NgForm;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private permissionService: PermissionService,
        private approveRegistrationsResource: ApproveRegistrationsResource,
        private alertMessageService: AlertMessageService,
        private pageHandlingService: PageHandlingService
    ) {
    }

    edit(editState: SignUpEditType) {
        this.editState = editState;
    }

    cancel() {
        this.approveRegistrationDto = JSON.parse(JSON.stringify(this.originalModel)) as ApproveRegistrationSearchDto;
        this.editState = this.signUpEditType.disabled;
    }

    update() {
        if (this.form.valid && this.editState !== this.signUpEditType.disabled) {
            this.signUpPending = true;

            if (this.editState === this.signUpEditType.userName) {
                this.approveRegistrationsResource
                    .signUpSupplierEdit(this.approveRegistrationDto)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.signUpPending = false;
                            this.pageHandlingService.scrollToTop();
                            return throwError(() => err);
                        })
                    )
                    .subscribe(() => {
                        this.signUpPending = false;
                        this.pageHandlingService.scrollToTop();
                        const alertMessage = new AlertMessageDto('successTexts.signUpSupplierEdit', 'fa-solid fa-envelope', null, 'bg-success text-light');
                        this.alertMessageService.show(alertMessage);
                        this.router.navigate(['/approveRegistrations']);
                    });
            } else {
                this.approveRegistrationsResource
                    .updateRepresentativeInfo(this.approveRegistrationDto)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.signUpPending = false;
                            this.pageHandlingService.scrollToTop();
                            return throwError(() => err);
                        })
                    )
                    .subscribe(() => {
                        this.signUpPending = false;
                        this.pageHandlingService.scrollToTop();
                        this.router.navigate(['/approveRegistrations']);
                    });
            }
        }
    }

    ngOnInit() {
        this.loadingData = true;
        this.route.params.subscribe(p => {
            this.approveRegistrationsResource.getById(p['id'])
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingData = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.approveRegistrationDto = e;
                    this.originalModel = JSON.parse(JSON.stringify(e)) as ApproveRegistrationSearchDto;
                    this.hasApprovalRegistrationWritePermission = this.permissionService.verifyUnitPermission(approvalRegistrationWritePermission, [[nacidAlias, null]]);
                    this.loadingData = false;
                });
        });
    }
}