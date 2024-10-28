import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierOfferingResource } from "src/suppliers/resources/supplier-offering.resource";

@Component({
    templateUrl: './supplier-offering-delete-modal.component.html'
})
export class SupplierOfferingDeleteModalComponent {

    @Input() supplierOfferingId: number;
    @Input() supplierId: number;

    supplierOfferingDto: SupplierOfferingDto = new SupplierOfferingDto();
    loadingData = false;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        this.decline();
    }

    constructor(private resource: SupplierOfferingResource,
        private activeModal: NgbActiveModal) {
    }

    accept() {
        this.activeModal.close(true);
    }

    decline() {
        this.activeModal.close(false);
    }

    ngOnInit() {
        this.loadingData = true;
        this.resource.getById(this.supplierId, this.supplierOfferingId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe((result: SupplierOfferingDto) => {
                this.loadingData = false;
                this.supplierOfferingDto = result;
            })
    }
}