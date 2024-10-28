import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { SupplierOfferingEquipmentDto } from "src/suppliers/dtos/junctions/supplier-offering-equipment.dto";
import { SupplierEquipmentDto } from "src/suppliers/dtos/supplier-equipment.dto";
import { SupplierEquipmentResource } from "src/suppliers/resources/supplier-equipment.resource";

@Component({
    selector: 'supplier-equipment-add-modal',
    templateUrl: './supplier-equipment-add-modal.component.html'
})
export class SupplierEquipmentAddModalComponent implements OnInit {

    addEquipmentPending = false;
    supplierEquipmentDto: SupplierEquipmentDto = new SupplierEquipmentDto();

    @ViewChild(NgForm) form: NgForm;

    @Input() supplierId: number;
    @Input() includeEquipmentOfferings = true;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.addEquipmentPending) {
            this.decline();
        }
    }

    constructor(
        private resource: SupplierEquipmentResource,
        private activeModal: NgbActiveModal) {
    }

    add() {
        if (this.form.valid) {
            this.addEquipmentPending = true;
            this.resource
                .create(this.supplierId, this.supplierEquipmentDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.addEquipmentPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.addEquipmentPending = false;
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
        this.supplierEquipmentDto.supplierId = this.supplierId;
    }
}