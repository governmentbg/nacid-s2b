import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { PermissionService } from "src/auth/services/permission.service";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { catchError, throwError } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { IsActiveDto } from "src/shared/dtos/is-active.dto";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierOfferingResource } from "src/suppliers/resources/supplier-offering.resource";
import { SupplierOfferingPermissionService } from "src/suppliers/services/supplier-offering-permission.service";
import { supplierOfferingDeletePermission, supplierOfferingWritePermission } from "src/auth/constants/permission.constants";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { SupplierOfferingEditModalComponent } from "./supplier-offering/modals/supplier-offering-edit-modal.component";
import { SupplierOfferingDeleteModalComponent } from "./supplier-offering/modals/supplier-offering-delete-modal.component";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'tr[supplier-offering-tr]',
    templateUrl: './supplier-offering-tr.component.html'
})
export class SupplierOfferingTrComponent implements OnInit {

    hasSupplierOfferingsWritePermission = false;
    hasSupplierOfferingsDeletePermission = false;

    @Input() supplierOffering: SupplierOfferingDto;
    @Input() supplier: SupplierDto;

    @Output() triggerRemove: EventEmitter<void> = new EventEmitter<void>();

    deleteSupplierOfferingPending = false;
    changeIsActiveSupplierOfferingPending = false;

    supplierType = SupplierType;

    constructor(
        private resource: SupplierOfferingResource,
        private supplierOfferingPermissionService: SupplierOfferingPermissionService,
        private modalService: NgbModal,
        public configuration: Configuration
    ) {
    }

    openEditSupplierOffering() {
        const modal = this.modalService.open(SupplierOfferingEditModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierOfferingId = this.supplierOffering.id;
        modal.componentInstance.supplierId = this.supplier.id;

        return modal.result.then((updatedSupplierOfferingDto: SupplierOfferingDto) => {
            if (updatedSupplierOfferingDto) {
                this.supplierOffering = updatedSupplierOfferingDto;
            }
        });
    }

    deleteSupplierOffering() {
        const modal = this.modalService.open(SupplierOfferingDeleteModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.supplierOfferingId = this.supplierOffering.id;
        modal.componentInstance.supplierId = this.supplier.id;

        return modal.result.then((ok: boolean) => {
            if (ok) {
                this.deleteSupplierOfferingPending = true;
                this.resource.delete(this.supplier.id, this.supplierOffering.id)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.deleteSupplierOfferingPending = false;
                            return throwError(() => err);
                        })
                    )
                    .subscribe(() => {
                        this.triggerRemove.emit();
                        this.deleteSupplierOfferingPending = false;
                    });
            }
        });
    }

    changeIsActive() {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.text = 'root.modals.changeState';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure';
        if (this.supplierOffering.isActive) {
            modal.componentInstance.title = 'supplierOfferings.modals.deactivateTitle';
        } else {
            modal.componentInstance.title = 'supplierOfferings.modals.activateTitle';
        }

        return modal.result.then((ok: boolean) => {
            if (ok) {
                this.changeIsActiveSupplierOfferingPending = true;

                var isActiveDto = new IsActiveDto();
                isActiveDto.id = this.supplierOffering.id;
                isActiveDto.isActive = !this.supplierOffering.isActive;

                this.resource.changeIsActive(this.supplier.id, isActiveDto)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.changeIsActiveSupplierOfferingPending = false;
                            return throwError(() => err);
                        })
                    )
                    .subscribe((isActive: boolean) => {
                        this.supplierOffering.isActive = isActive;
                        this.changeIsActiveSupplierOfferingPending = false;
                    });
            }
        });
    }

    ngOnInit() {
        this.hasSupplierOfferingsWritePermission = this.supplierOfferingPermissionService.verifyOfferingPermissionException(supplierOfferingWritePermission, this.supplier, this.supplierOffering);
        this.hasSupplierOfferingsDeletePermission = this.supplierOfferingPermissionService.verifyOfferingPermissionException(supplierOfferingDeletePermission, this.supplier, this.supplierOffering);
    }
}