import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
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
    selector: 'supplier-offering-add-modal',
    templateUrl: './supplier-offering-add-modal.component.html'
})
export class SupplierOfferingAddModalComponent implements OnInit {

    addOfferingPending = false;
    supplierOfferingDto: SupplierOfferingDto = new SupplierOfferingDto();

    @ViewChild(NgForm) form: NgForm;

    @Input() supplierId: number;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        if (!this.addOfferingPending) {
            this.decline();
        }
    }

    constructor(
        private resource: SupplierOfferingResource,
        private activeModal: NgbActiveModal) {
    }

    add() {
        if (this.form.valid) {
            this.addOfferingPending = true;
            this.resource
                .create(this.supplierId, this.supplierOfferingDto)
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.addOfferingPending = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.addOfferingPending = false;
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
        this.supplierOfferingDto.files.splice(index, 1);
    }

    ngOnInit() {
        let smartSpecialization = new SupplierOfferingSmartSpecializationDto();
        smartSpecialization.type = SupplierOfferingSmartSpecializationType.primary;
        this.supplierOfferingDto.smartSpecializations.push(smartSpecialization);


        this.supplierOfferingDto.supplierId = this.supplierId;
    }
}