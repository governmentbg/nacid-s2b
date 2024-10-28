import { HttpErrorResponse } from "@angular/common/http";
import { Component, Input, OnInit } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { complexAlias, nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { PermissionService } from "src/auth/services/permission.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { supplierEquipmentCreatePermission, supplierEquipmentDeletePermission, supplierEquipmentWritePermission } from "src/auth/constants/permission.constants";
import { SupplierEquipmentDto } from "src/suppliers/dtos/supplier-equipment.dto";
import { SupplierEquipmentResource } from "src/suppliers/resources/supplier-equipment.resource";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { SupplierEquipmentEditModalComponent } from "./supplier-equipment/modals/supplier-equipment-edit-modal.component";
import { SupplierEquipmentAddModalComponent } from "./supplier-equipment/modals/supplier-equipment-add-modal.component";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'supplier-equipment',
    templateUrl: './supplier-equipment.component.html',
})
export class SupplierEquipmentComponent implements OnInit {

    hasSupplierEquipmentCreatePermission = false;
    hasSupplierEquipmentWritePermission = false;
    hasSupplierEquipmentDeletePermission = false;

    supplierType = SupplierType;
    loadingData = false;

    supplierEquipment: SupplierEquipmentDto[] = [];
    deleteSupplierEquipmentPending: boolean[] = [];

    @Input() supplier: SupplierDto;

    constructor(
        private resource: SupplierEquipmentResource,
        private permissionService: PermissionService,
        private modalService: NgbModal,
        public configuration: Configuration
    ) {
    }

    openAddSupplierEquipment() {
        const modal = this.modalService.open(SupplierEquipmentAddModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierId = this.supplier.id;

        return modal.result.then((newSupplierEquipmentDto: SupplierEquipmentDto) => {
            if (newSupplierEquipmentDto) {
                this.supplierEquipment.unshift(newSupplierEquipmentDto);
            }
        });
    }

    openEditSupplierEquipment(equipment: SupplierEquipmentDto, index: number) {
        const modal = this.modalService.open(SupplierEquipmentEditModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierEquipmentId = equipment.id;
        modal.componentInstance.supplierId = this.supplier.id;

        return modal.result.then((updatedSupplierEquipmentDto: SupplierEquipmentDto) => {
            if (updatedSupplierEquipmentDto) {
                this.supplierEquipment[index] = updatedSupplierEquipmentDto;
            }
        });
    }

    deleteSupplierEquipment(equipmentId: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.text = 'supplierEquipment.modals.deleteEquipment';
        modal.componentInstance.text2 = 'supplierEquipment.modals.confirmDelete';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure';
        modal.componentInstance.title = 'supplierEquipment.modals.deleteTitle';

        return modal.result.then((ok: boolean) => {
            if (ok) {
                this.deleteSupplierEquipmentPending[index] = true;
                this.resource.delete(this.supplier.id, equipmentId)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.deleteSupplierEquipmentPending[index] = false;
                            return throwError(() => err);
                        })
                    )
                    .subscribe(() => {
                        this.supplierEquipment.splice(index, 1);
                        this.deleteSupplierEquipmentPending[index] = false;
                    });
            }
        });
    }

    private getEquipmentBySupplier(supplierId: number) {
        this.resource
            .getBySupplier(supplierId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.supplierEquipment = e;
                this.loadingData = false;
            });
    }

    ngOnInit() {
        this.loadingData = true;
        this.hasSupplierEquipmentCreatePermission = (this.supplier.type === this.supplierType.institution
            ? this.permissionService.verifyUnitPermission(supplierEquipmentCreatePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
            : this.permissionService.verifyUnitPermission(supplierEquipmentCreatePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));
        this.hasSupplierEquipmentWritePermission = (this.supplier.type === this.supplierType.institution
            ? this.permissionService.verifyUnitPermission(supplierEquipmentWritePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
            : this.permissionService.verifyUnitPermission(supplierEquipmentWritePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));
        this.hasSupplierEquipmentDeletePermission = (this.supplier.type === this.supplierType.institution
            ? this.permissionService.verifyUnitPermission(supplierEquipmentDeletePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
            : this.permissionService.verifyUnitPermission(supplierEquipmentDeletePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));

        this.getEquipmentBySupplier(this.supplier.id);
    }
}