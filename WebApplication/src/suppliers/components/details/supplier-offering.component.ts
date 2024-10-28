import { HttpErrorResponse } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { complexAlias, nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { supplierOfferingCreatePermission, supplierOfferingDeletePermission, supplierOfferingWritePermission } from "src/auth/constants/permission.constants";
import { PermissionService } from "src/auth/services/permission.service";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { OfferingType } from "src/suppliers/enums/offering-type.enum";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { SupplierOfferingResource } from "src/suppliers/resources/supplier-offering.resource";
import { SupplierOfferingAddModalComponent } from "./supplier-offering/modals/supplier-offering-add-modal.component";
import { SupplierOfferingPermissionService } from "src/suppliers/services/supplier-offering-permission.service";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'supplier-offering',
    templateUrl: './supplier-offering.component.html',
})
export class SupplierOfferingComponent {

    hasSupplierOfferingsCreatePermission = false;
    hasSupplierOfferingsWritePermission = false;
    hasSupplierOfferingsDeletePermission = false;


    supplierOffering: SupplierOfferingDto[] = [];

    loadingData = false;
    offeringType = OfferingType;
    supplierType = SupplierType;

    supplier: SupplierDto;
    @Input('supplier')
    set supplierSetter(supplier: SupplierDto) {
        this.loadingData = true;
        this.supplier = supplier;
        this.hasSupplierOfferingsCreatePermission =
            (this.supplier.type === this.supplierType.institution
                ? this.permissionService.verifyUnitPermission(supplierOfferingCreatePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
                : this.permissionService.verifyUnitPermission(supplierOfferingCreatePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));

        this.getOfferingBySupplier(this.supplier.id);
    }

    constructor(
        private resource: SupplierOfferingResource,
        private permissionService: PermissionService,
        private modalService: NgbModal,
        private supplierOfferingPermissionService: SupplierOfferingPermissionService,
        public configuration: Configuration
    ) {
    }

    openAddSupplierOffering() {
        const modal = this.modalService.open(SupplierOfferingAddModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierId = this.supplier.id;

        return modal.result.then((newSupplierOfferingDto: SupplierOfferingDto) => {
            if (newSupplierOfferingDto) {
                this.supplierOffering.unshift(newSupplierOfferingDto);
            }
        });
    }

    removeSupplierOffering(index: number) {
        this.supplierOffering.splice(index, 1);
    }


    private getOfferingBySupplier(supplierId: number) {
        this.resource
            .getBySupplier(supplierId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.supplierOffering = e;
                this.loadingData = false;

                this.hasSupplierOfferingsWritePermission = this.supplierOffering.some(offering =>
                    this.supplierOfferingPermissionService.verifyOfferingPermissionException(supplierOfferingWritePermission, this.supplier, offering)
                );

                this.hasSupplierOfferingsDeletePermission = this.supplierOffering.some(offering =>
                    this.supplierOfferingPermissionService.verifyOfferingPermissionException(supplierOfferingDeletePermission, this.supplier, offering))
            });
    }
}