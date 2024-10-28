import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError, throwError } from 'rxjs';
import { VoucherRequestStateDto } from 'src/voucher-requests/dtos/voucher-request-state.dto';
import { VoucherRequestDto } from 'src/voucher-requests/dtos/voucher-request.dto';
import { VoucherRequestState } from 'src/voucher-requests/enums/voucher-request-state.enum';
import { VoucherRequestResource } from 'src/voucher-requests/resources/voucher-request.resource';

@Component({
  selector: 'app-voucher-request-decline-code-modal',
  templateUrl: './voucher-request-decline-code-modal.component.html'
})
export class VoucherRequestDeclineCodeComponent {
  voucherRequestState = VoucherRequestState;

  declineCodePending = false;

  @Input() state: VoucherRequestState;
  @Input() voucherRequest: VoucherRequestDto
  
  constructor(
    public activeModal: NgbActiveModal,
    private resource: VoucherRequestResource
  ) { }

  declineCode() {
    this.declineCodePending = true;

    const voucherRequestStateDto = new VoucherRequestStateDto();
    voucherRequestStateDto.requestCompanyId = this.voucherRequest.requestCompanyId;
    voucherRequestStateDto.supplierOfferingId = this.voucherRequest.supplierOfferingId;
    voucherRequestStateDto.state = this.state;

    this.resource
        .declineCode(voucherRequestStateDto)
        .pipe(
            catchError((err: HttpErrorResponse) => {
                this.declineCodePending = false;
                return throwError(() => err);
            })
        )
        .subscribe(e => {
            this.declineCodePending = false;
            this.voucherRequest.state = e.state;
            this.activeModal.close(e);
        });
}

  onCancel(): void {
    this.activeModal.close();
  }
}
