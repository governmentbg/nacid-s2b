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
    selector: 'supplier-team-edit-modal',
    templateUrl: './supplier-team-edit-modal.component.html'
})
export class SupplierTeamEditModalComponent implements OnInit {

    @Input() supplierTeamId: number;
    @Input() supplierId: number;
    @Input() supplierType: SupplierType;
    @Input() supplierInstitutionId: number;

    editTeamPending = false;
    supplierTeamDto: SupplierTeamDto = new SupplierTeamDto();
    originalModel = new SupplierTeamDto();
    isEditMode = false;
    loadingData = false;

    @ViewChild(NgForm) form: NgForm;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.editTeamPending) {
            this.decline();
        }
    }

    constructor(
        private resource: SupplierTeamResource,
        private activeModal: NgbActiveModal) {
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.supplierTeamDto)) as SupplierTeamDto;
        this.isEditMode = true;
    }

    cancel() {
        this.supplierTeamDto = JSON.parse(JSON.stringify(this.originalModel)) as SupplierTeamDto;
        this.isEditMode = false;
        this.originalModel = null;
    }

    save() {
        if (this.form.valid) {
            this.editTeamPending = true;
            this.resource
                .update(this.supplierId, this.supplierTeamDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.editTeamPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.editTeamPending = false;
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
        this.loadingData = true;
        this.resource.getById(this.supplierId, this.supplierTeamId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.decline();
                    return throwError(() => err);
                })
            )
            .subscribe((result: SupplierTeamDto) => {
                this.loadingData = false;
                this.supplierTeamDto = result;
            })
    }
}