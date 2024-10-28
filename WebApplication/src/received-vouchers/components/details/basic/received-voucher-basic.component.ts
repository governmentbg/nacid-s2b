import { Component, Input } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";

@Component({
    selector: 'received-voucher-basic',
    templateUrl: './received-voucher-basic.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class ReceivedVoucherBasicComponent {

    @Input() receivedVoucherDto: ReceivedVoucherDto = new ReceivedVoucherDto();
    @Input() isEditMode = false;

    receivedVoucherState = ReceivedVoucherState;

    constructor(public translateService: TranslateService) {
    }

    changedSupplier(supplierId: number) {
        this.receivedVoucherDto.supplierId = supplierId;
        this.receivedVoucherDto.offering = null;
        this.receivedVoucherDto.offeringId = null;

        if (!supplierId) {
            this.receivedVoucherDto.offeringAdditionalPayment = false;
            this.receivedVoucherDto.offeringClarifications = null;
            this.receivedVoucherDto.secondSupplier = null;
            this.receivedVoucherDto.secondSupplierId = null;
            this.receivedVoucherDto.secondOffering = null;
            this.receivedVoucherDto.secondOfferingId = null;
            this.receivedVoucherDto.secondOfferingAdditionalPayment = false;
            this.receivedVoucherDto.secondOfferingClarifications = null;
        }
    }

    changedSecondSupplier(secondSupplierId: number) {
        this.receivedVoucherDto.secondSupplierId = secondSupplierId;
        this.receivedVoucherDto.secondOffering = null;
        this.receivedVoucherDto.secondOfferingId = null;

        if (!secondSupplierId) {
            this.receivedVoucherDto.secondOfferingAdditionalPayment = false;
            this.receivedVoucherDto.secondOfferingClarifications = null;
        }
    }
}