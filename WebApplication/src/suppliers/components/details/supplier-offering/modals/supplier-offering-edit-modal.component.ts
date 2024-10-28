import { HttpErrorResponse } from "@angular/common/http";
import { Component, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { SupplierOfferingEquipmentDto } from "src/suppliers/dtos/junctions/supplier-offering-equipment.dto";
import { SupplierOfferingSmartSpecializationDto } from "src/suppliers/dtos/junctions/supplier-offering-smart-specialization.dto";
import { SupplierOfferingTeamDto } from "src/suppliers/dtos/junctions/supplier-offering-team.dto";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierOfferingSmartSpecializationType } from "src/suppliers/enums/supplier-offering-smart-specialization-type.enum";
import { SupplierOfferingResource } from "src/suppliers/resources/supplier-offering.resource";

@Component({
    selector: 'supplier-offering-edit-modal',
    templateUrl: './supplier-offering-edit-modal.component.html'
})
export class SupplierOfferingEditModalComponent implements OnInit {

    @Input() supplierOfferingId: number;
    @Input() supplierId: number;
    
    editOfferingPending = false;
    supplierOfferingDto: SupplierOfferingDto = new SupplierOfferingDto();
    originalModel = new SupplierOfferingDto();
    isEditMode = false;
    loadingData = false;

    @ViewChild(NgForm) form: NgForm;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.editOfferingPending) {
            this.decline();
        }
    }

    constructor(
        private resource: SupplierOfferingResource,
        private activeModal: NgbActiveModal) {
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.supplierOfferingDto)) as SupplierOfferingDto;
        this.isEditMode = true;
    }

    cancel() {
        this.supplierOfferingDto = JSON.parse(JSON.stringify(this.originalModel)) as SupplierOfferingDto;
        this.isEditMode = false;
        this.originalModel = null;
    }

    save() {
        if (this.form.valid) {
            this.editOfferingPending = true;
            this.resource
                .update(this.supplierId, this.supplierOfferingDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.editOfferingPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.editOfferingPending = false;
                    this.activeModal.close(e);
                });
        }
    }

    decline() {
        this.activeModal.close(false);
    }

    addSoSmartSpecialization() {
        let smartSpecialization = new SupplierOfferingSmartSpecializationDto();
        smartSpecialization.type = SupplierOfferingSmartSpecializationType.secondary;
        this.supplierOfferingDto.smartSpecializations.push(smartSpecialization);
    }

    addSoTeam() {
        let soTeam = new SupplierOfferingTeamDto();
        this.supplierOfferingDto.supplierOfferingTeams.push(soTeam);
    }

    addSoEquipment() {
        let soEquipment = new SupplierOfferingEquipmentDto();
        this.supplierOfferingDto.supplierOfferingEquipment.push(soEquipment);
    }

    addSoFile() {
        this.supplierOfferingDto.files.push(null)
    }

    eraseSoSmartSpecialization(index: number) {
        this.supplierOfferingDto.smartSpecializations.splice(index, 1);
    }

    eraseSoTeam(index: number) {
        this.supplierOfferingDto.supplierOfferingTeams.splice(index, 1);
    }

    eraseSoEquipment(index: number) {
        this.supplierOfferingDto.supplierOfferingEquipment.splice(index, 1);
    }

    eraseSoFile(index: number) {
        this.supplierOfferingDto.files.splice(index, 1)
    }

    ngOnInit() {
        this.loadingData = true;
        this.resource.getById(this.supplierId, this.supplierOfferingId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    this.decline();
                    return throwError(() => err);
                })
            )
            .subscribe((result: SupplierOfferingDto) => {
                this.loadingData = false;
                this.supplierOfferingDto = result;
            })
    }
}