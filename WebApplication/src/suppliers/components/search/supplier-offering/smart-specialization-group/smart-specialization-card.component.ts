import { Component, Input, ViewChildren } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { CollapsableLabelComponent } from "src/shared/components/collapsable-label/collapsable-label.component";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { SmartSpecializationRootGroupDto } from "src/suppliers/dtos/search/supplier-offering-group/smart-specialization-root-group.dto";

@Component({
    selector: 'smart-specialization-card',
    templateUrl: './smart-specialization-card.component.html'
})
export class SmartSpecializationCardComponent {

    @Input() smartSpecializationRootGroup: SmartSpecializationRootGroupDto;

    @ViewChildren(CollapsableLabelComponent) collapsableLables: CollapsableLabelComponent[];

    constructor(
        public translateService: TranslateService,
        private router: Router,
        private pageHandlingService: PageHandlingService
    ) {
    }

    openReadSupplierOffering(supplierId: number, id: number) {
        this.pageHandlingService.scrollToTop();
        this.router.navigate([`/suppliers/${supplierId}/offerings/${id}`]);
    }

    collapseAll(index: number) {
        if (this.collapsableLables && this.collapsableLables.length > 0) {
            this.collapsableLables.forEach((element, i) => {
                if (index !== i) {
                    element.isCollapsed = true;
                }
            });
        }
    }
}