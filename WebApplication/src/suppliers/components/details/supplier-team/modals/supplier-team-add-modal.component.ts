import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { SupplierOfferingTeamDto } from "src/suppliers/dtos/junctions/supplier-offering-team.dto";
import { SupplierTeamDto } from "src/suppliers/dtos/supplier-team.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { SupplierTeamResource } from "src/suppliers/resources/supplier-team.resource";

@Component({
    selector: 'supplier-team-add-modal',
    templateUrl: './supplier-team-add-modal.component.html'
})
export class SupplierTeamAddModalComponent implements OnInit {

    addToTeamPending = false;
    supplierTeamDto: SupplierTeamDto = new SupplierTeamDto();

    @ViewChild(NgForm) form: NgForm;

    @Input() supplierId: number;
    @Input() supplierType: SupplierType;
    @Input() supplierInstitutionId: number;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.addToTeamPending) {
            this.decline();
        }
    }

    constructor(
        private resource: SupplierTeamResource,
        private activeModal: NgbActiveModal) {
    }

    add() {
        if (this.form.valid) {
            this.addToTeamPending = true;
            this.resource
                .create(this.supplierId, this.supplierTeamDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.addToTeamPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.addToTeamPending = false;
                    this.activeModal.close(e);
                });
        }
    }

    decline() {
        this.activeModal.close(false);
    }

    addSoTeam() {
        this.supplierTeamDto.supplierOfferingTeams.push(new SupplierOfferingTeamDto());
    }

    eraseSoTeam(index: number) {
        this.supplierTeamDto.supplierOfferingTeams.splice(index, 1);
    }

    ngOnInit() {
        this.supplierTeamDto.supplierId = this.supplierId;
        this.supplierTeamDto.isActive = true;
    }
}