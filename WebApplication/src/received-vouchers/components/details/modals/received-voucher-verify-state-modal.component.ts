import { Component, HostListener } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";

@Component({
    templateUrl: './received-voucher-verify-state-modal.component.html'
})
export class ReceivedVoucherVerifyStateModalComponent {

    receivedVoucherState = ReceivedVoucherState;

    @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
        this.decline();
    }

    constructor(private activeModal: NgbActiveModal) {
    }

    accept(state: ReceivedVoucherState) {
        this.activeModal.close(state);
    }

    decline() {
        this.activeModal.close(null);
    }
}