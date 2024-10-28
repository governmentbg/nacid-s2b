import { HttpErrorResponse } from "@angular/common/http";
import { Component, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { ReceivedVoucherResource } from "src/received-vouchers/resources/received-voucher.resource";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { ReceivedVoucherVerifyStateModalComponent } from "./modals/received-voucher-verify-state-modal.component";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { ReceivedVoucherCountResource } from "src/received-vouchers/resources/received-voucher-count.resource";

@Component({
    selector: 'received-voucher-create',
    templateUrl: './received-voucher-create.component.html'
})
export class ReceivedVoucherCreateComponent {

    createPending = false;
    receivedVoucherDto: ReceivedVoucherDto = new ReceivedVoucherDto();

    @ViewChild(NgForm) form: NgForm;

    constructor(
        private resource: ReceivedVoucherResource,
        private pageHandlingService: PageHandlingService,
        private router: Router,
        private modalService: NgbModal,
        private receivedVoucherCountResource: ReceivedVoucherCountResource
    ) {
    }

    create() {
        if (this.form.valid) {
            if (this.receivedVoucherDto.file != null && this.receivedVoucherDto.supplierId != null && this.receivedVoucherDto.offeringId != null) {
                const modal = this.modalService.open(ReceivedVoucherVerifyStateModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });

                modal.result.then((state: ReceivedVoucherState) => {
                    if (state) {
                        if (state === ReceivedVoucherState.completed) {
                            this.createData(ReceivedVoucherState.completed);
                        } else if (state === ReceivedVoucherState.draft) {
                            this.createData(ReceivedVoucherState.draft);
                        }
                    }
                });
            } else {
                const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
                modal.componentInstance.text = 'receivedVouchers.modals.draftTitle';
                modal.componentInstance.acceptButton = 'root.buttons.yesSure';

                modal.result.then((ok: boolean) => {
                    if (ok) {
                        this.createData(ReceivedVoucherState.draft);
                    }
                });
            }
        }
    }

    decline() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/receivedVouchers']);
    }

    private createData(state: ReceivedVoucherState) {
        this.createPending = true;
        this.receivedVoucherDto.state = state;
        this.resource
            .create(this.receivedVoucherDto)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.createPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(() => {
                this.createPending = false;
                this.receivedVoucherCountResource.incrementVoucherCount();
                this.decline();
            });
    }
}
