import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { VoucherRequestDto } from "src/voucher-requests/dtos/voucher-request.dto";
import { VoucherRequestState } from "src/voucher-requests/enums/voucher-request-state.enum";

@Component({
    selector: 'tr[voucher-request-search-tr]',
    templateUrl: './voucher-request-search-tr.component.html'
})
export class VoucherRequestSearchTrComponent {

    voucherRequestState = VoucherRequestState;

    @Input() voucherRequest = new VoucherRequestDto();

    constructor(
        private router: Router,
        private pageHandlingService: PageHandlingService
    ) {
    }

    open() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/voucherRequests', this.voucherRequest.id]);
    }

    getSupplierOffering() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate([`/suppliers/${this.voucherRequest.supplierOffering.supplierId}/offerings/${this.voucherRequest.supplierOfferingId}`]);
    }

    getCompany() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/companies', this.voucherRequest.requestCompanyId]);
    }
}