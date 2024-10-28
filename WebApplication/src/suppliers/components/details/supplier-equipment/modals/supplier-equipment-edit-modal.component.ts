import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { SupplierOfferingEquipmentDto } from "src/suppliers/dtos/junctions/supplier-offering-equipment.dto";
import { SupplierEquipmentDto } from "src/suppliers/dtos/supplier-equipment.dto";
import { SupplierEquipmentResource } from "src/suppliers/resources/supplier-equipment.resource";

@Component({
    selector: 'supplier-equipment-edit-modal',
    templateUrl: './supplier-equipment-edit-modal.component.html'
})
export class SupplierEquipmentEditModalComponent implements OnInit {

    @Input() supplierEquipmentId: number;
    @Input() supplierId: number;

    supplierEquipmentDto: SupplierEquipmentDto = new SupplierEquipmentDto();
    originalModel = new SupplierEquipmentDto();

    editEquipmentPending = false;
    isEditMode = false;
    loadingData = false;

    @ViewChild(NgForm) form: NgForm;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.editEquipmentPending) {
            this.decline();
        }
    }

    constructor(
        private resource: SupplierEquipmentResource,
        private activeModal: NgbActiveModal) {
    }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.supplierEquipmentDto)) as SupplierEquipmentDto;
        this.isEditMode = true;
    }

    cancel() {
        this.supplierEquipmentDto = JSON.parse(JSON.stringify(this.originalModel)) as SupplierEquipmentDto;
        this.isEditMode = false;
        this.originalModel = null;
    }

    save() {
        if (this.form.valid) {
            this.editEquipmentPending = true;
            this.resource
                .update(this.supplierId, this.supplierEquipmentDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.editEquipmentPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.editEquipmentPending = false;
                    this.activeModal.close(e);
                });
        }
    }

    decline() {
        this.activeModal.close(false);
    }

    addSoEquipment() {
        this.supplierEquipmentDto.supplierOfferingEquipment.push(new SupplierOfferingEquipmentDto());
    }

    eraseSoEquipment(index: number) {
        this.supplierEquipmentDto.supplierOfferingEquipment.splice(index, 1);
    }

    ngOnInit() {
        this.loadingData = true;
        this.resource.getById(this.supplierId, this.supplierEquipmentId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.decline();
                    return throwError(() => err);
                })
            )
            .subscribe((result: SupplierEquipmentDto) => {
                this.loadingData = false;
                this.supplierEquipmentDto = result;
            })
    }
}