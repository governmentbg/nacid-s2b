import { HttpErrorResponse } from "@angular/common/http";
import { Component, HostListener, Input, OnInit } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { UserContextService } from "src/auth/services/user-context.service";
import { ReceivedVoucherCertificateDto } from "src/received-vouchers/dtos/received-voucher-certificate.dto";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherCertificateResource } from "src/received-vouchers/resources/received-voucher-certificate.resource";

@Component({
    templateUrl: './received-voucher-certificate-modal.component.html'
})
export class ReceivedVoucherCertificateModalComponent implements OnInit {

    selectedVoucherCertificate = new ReceivedVoucherCertificateDto();
    receivedVoucherCertificateSelections: ReceivedVoucherCertificateDto[] = [];

    generatingCertificatePending = false;

    @Input() receivedVoucherDto: ReceivedVoucherDto;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        this.close(null);
    }

    constructor(
        private resource: ReceivedVoucherCertificateResource,
        private activeModal: NgbActiveModal,
        private userContextService: UserContextService) {
    }

    changeSelectedCertificate(selectedVoucherCertificate: ReceivedVoucherCertificateDto) {
        this.selectedVoucherCertificate = JSON.parse(JSON.stringify(selectedVoucherCertificate)) as ReceivedVoucherCertificateDto;
    }

    accept() {
        this.generatingCertificatePending = true;
        this.resource
            .generateCertificate(this.selectedVoucherCertificate)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.generatingCertificatePending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.generatingCertificatePending = false;
                this.activeModal.close(e);
            });
    }

    close(selectedCertificate: ReceivedVoucherCertificateDto) {
        this.activeModal.close(selectedCertificate);
    }

    ngOnInit() {
        const generatedCertificates = this.receivedVoucherDto.certificates.map(e => e.offeringId);

        if (this.receivedVoucherDto.secondOfferingId && this.receivedVoucherDto.secondOfferingId > 0 ? generatedCertificates?.length > 1 : generatedCertificates?.length > 0) {
            this.close(null);
        }

        const userSupplierIds = this.userContextService.userContext.organizationalUnits.filter(e => e.supplierId !== null).map(e => e.supplierId);

        if (!generatedCertificates.includes(this.receivedVoucherDto.offeringId) && userSupplierIds.includes(this.receivedVoucherDto.supplierId)) {
            const certificationSelect = new ReceivedVoucherCertificateDto();
            certificationSelect.offeringId = this.receivedVoucherDto.offeringId;
            certificationSelect.offering = this.receivedVoucherDto.offering;
            certificationSelect.receivedVoucherId = this.receivedVoucherDto.id;
            certificationSelect.supplier = this.receivedVoucherDto.supplier;
            certificationSelect.supplierId = this.receivedVoucherDto.supplierId;
            this.receivedVoucherCertificateSelections.push(certificationSelect);
        }

        if (this.receivedVoucherDto.secondOfferingId && !generatedCertificates.includes(this.receivedVoucherDto.secondOfferingId) && userSupplierIds.includes(this.receivedVoucherDto.secondSupplierId)) {
            const certificationSelect = new ReceivedVoucherCertificateDto();
            certificationSelect.offeringId = this.receivedVoucherDto.secondOfferingId;
            certificationSelect.offering = this.receivedVoucherDto.secondOffering;
            certificationSelect.receivedVoucherId = this.receivedVoucherDto.id;
            certificationSelect.supplier = this.receivedVoucherDto.secondSupplier;
            certificationSelect.supplierId = this.receivedVoucherDto.secondSupplierId;
            this.receivedVoucherCertificateSelections.push(certificationSelect);
        }

        if (!this.receivedVoucherCertificateSelections || this.receivedVoucherCertificateSelections?.length < 1) {
            this.close(null);
        }

        this.changeSelectedCertificate(this.receivedVoucherCertificateSelections[0]);
    }
}