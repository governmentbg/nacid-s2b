import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError, throwError } from 'rxjs';
import { ReceivedVoucherDto } from 'src/received-vouchers/dtos/received-voucher.dto';
import { ReceivedVoucherResource } from 'src/received-vouchers/resources/received-voucher.resource';

@Component({
  selector: 'app-received-voucher-terminate-modal',
  templateUrl: './received-voucher-terminate-modal.component.html'
})
export class TerminateReceivedVoucherModal {

  @Input() receivedVoucher: ReceivedVoucherDto;

  terminateDataPending = false;
  
  constructor(
    public activeModal: NgbActiveModal,
    private resource: ReceivedVoucherResource
  ) { }

  terminate() {
    this.terminateDataPending = true;
    this.resource
        .terminate(this.receivedVoucher.id)
        .pipe(
            catchError((err: HttpErrorResponse) => {
                this.terminateDataPending = false;
                return throwError(() => err);
            })
        )
        .subscribe(e => {
            this.receivedVoucher = e;
            this.terminateDataPending = false;
            this.activeModal.close(e);
        });
}

  onCancel(): void {
    this.activeModal.close();
  }
}
