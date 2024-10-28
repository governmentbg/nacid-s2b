import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SupplierOfferingSearchDto } from "src/suppliers/dtos/search/supplier-offering-group/supplier-offering-search.dto";

@Component({
    selector: 'supplier-offering-group-card',
    templateUrl: './supplier-offering-group-card.component.html'
})
export class SupplierOfferingGroupCardComponent {

    @Input() supplierOffering: SupplierOfferingSearchDto;

    constructor(
        private router: Router,
        private pageHandlingService: PageHandlingService
    ) {

    }

    openReadSupplierOffering(supplierId: number, id: number) {
        this.pageHandlingService.scrollToTop();
        this.router.navigate([`/suppliers/${supplierId}/offerings/${id}`]);
    }
}
