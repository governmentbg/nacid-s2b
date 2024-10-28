import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { CompanyAdditionalDto } from "src/companies/dtos/company-additional.dto";
import { CompanyAdditionalResource } from "src/companies/resources/company-additional.resource";

@Component({
    selector: 'company-additional-add-modal',
    templateUrl: './company-additional-add-modal.component.html'
})
export class CompanyAdditionalAddModalComponent implements OnInit {

    addAdditionalPending = false;
    companyAdditionalDto: CompanyAdditionalDto = new CompanyAdditionalDto();

    @ViewChild(NgForm) form: NgForm;

    @Input() companyId: number;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.addAdditionalPending) {
            this.decline();
        }
    }

    constructor(
        private resource: CompanyAdditionalResource,
        private activeModal: NgbActiveModal) {
    }

    add() {
        if (this.form.valid) {
            this.addAdditionalPending = true;
            this.resource
                .create(this.companyId, this.companyAdditionalDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.addAdditionalPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.addAdditionalPending = false;
                    this.activeModal.close(e);
                });
        }
    }

    decline() {
        this.activeModal.close(false);
    }

    ngOnInit() {
        this.companyAdditionalDto.id = this.companyId;
    }
}