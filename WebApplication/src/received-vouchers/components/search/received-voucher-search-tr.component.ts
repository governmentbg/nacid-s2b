import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { Configuration } from "src/app/configuration/configuration";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";

@Component({
    selector: 'tr[received-voucher-search-tr]',
    templateUrl: './received-voucher-search-tr.component.html'
})
export class ReceivedVoucherSearchTrComponent {

    receivedVoucherState = ReceivedVoucherState;

    @Input() receivedVoucher = new ReceivedVoucherDto();

    constructor(
        private router: Router,
        private pageHandlingService: PageHandlingService,
        public configuration: Configuration
    ) {
    }

    open() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/receivedVouchers', this.receivedVoucher.id]);
    }

    getSupplierOffering(supplierId: number, offeringId: number) {
        this.pageHandlingService.scrollToTop();
        this.router.navigate([`/suppliers/${supplierId}/offerings/${offeringId}`]);
    }

    getCompany() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/companies', this.receivedVoucher.companyId]);
    }
}