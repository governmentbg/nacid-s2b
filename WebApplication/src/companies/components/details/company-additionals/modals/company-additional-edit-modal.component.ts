import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { CompanyAdditionalDto } from "src/companies/dtos/company-additional.dto";
import { CompanyAdditionalResource } from "src/companies/resources/company-additional.resource";

@Component({
    selector: 'company-additional-edit-modal',
    templateUrl: './company-additional-edit-modal.component.html'
})
export class CompanyAdditionalEditModalComponent implements OnInit {

    @Input() companyId: number;

    editAdditionalPending = false;
    companyAdditionalDto: CompanyAdditionalDto = new CompanyAdditionalDto();
    originalModel = new CompanyAdditionalDto();
    isEditMode = false;
    loadingData = false;

    @ViewChild(NgForm) form: NgForm;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.editAdditionalPending) {
            this.decline();
        }
    }

    constructor(
        private resource: CompanyAdditionalResource,
        private activeModal: NgbActiveModal) {
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.companyAdditionalDto)) as CompanyAdditionalDto;
        this.isEditMode = true;
    }

    cancel() {
        this.companyAdditionalDto = JSON.parse(JSON.stringify(this.originalModel)) as CompanyAdditionalDto;
        this.isEditMode = false;
        this.originalModel = null;
    }

    save() {
        if (this.form.valid) {
            this.editAdditionalPending = true;
            this.resource
                .update(this.companyId, this.companyAdditionalDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.editAdditionalPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.editAdditionalPending = false;
                    this.activeModal.close(e);
                });
        }
    }

    decline() {
        this.activeModal.close(false);
    }

    ngOnInit() {
        this.loadingData = true;
        this.resource.getById(this.companyId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.decline();
                    return throwError(() => err);
                })
            )
            .subscribe((result: CompanyAdditionalDto) => {
                this.loadingData = false;
                this.companyAdditionalDto = result;
            })
    }
}