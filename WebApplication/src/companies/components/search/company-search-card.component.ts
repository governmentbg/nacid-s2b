import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { CompanyDto } from "src/companies/dtos/company.dto";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";

@Component({
    selector: 'company-search-card',
    templateUrl: './company-search-card.component.html'
})
export class CompanySearchCardComponent {

    @Input() company: CompanyDto;

    constructor(
        private router: Router,
        private pageHandlingService: PageHandlingService
    ) {
    }

    getCompany() {
        this.pageHandlingService.scrollToTop();
        this.router.navigate(['/companies', this.company.id]);
    }
}