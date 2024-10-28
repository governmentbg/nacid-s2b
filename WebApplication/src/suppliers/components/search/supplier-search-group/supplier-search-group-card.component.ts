import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PageHandlingService } from 'src/shared/services/page-handling/page-handling.service';
import { SupplierRootGroupDto } from 'src/suppliers/dtos/search/supplier-group/supplier-root-group.dto';

@Component({
  selector: 'supplier-search-group-card',
  templateUrl: './supplier-search-group-card.component.html',
  styleUrls: ['./supplier-search-group-card.styles.css']
})
export class SupplierSearchGroupCardComponent {

  @Input() supplier: SupplierRootGroupDto;

  showSmartSpecializations = false;

  constructor(
    public translateService: TranslateService,
    private pageHandlingService: PageHandlingService,
    private router: Router
  ) {
  }

  getSupplier() {
    if (this.supplier.id) {
      this.pageHandlingService.scrollToTop();
      this.router.navigate(['/suppliers', this.supplier.id]);
    }
  }
}
