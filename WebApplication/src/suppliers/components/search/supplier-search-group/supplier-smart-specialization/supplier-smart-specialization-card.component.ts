import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { PageHandlingService } from 'src/shared/services/page-handling/page-handling.service';
import { SmartSpecializationGroupDto } from 'src/suppliers/dtos/search/supplier-offering-group/smart-specialization-group.dto';

@Component({
  selector: 'supplier-smart-specialization-card',
  templateUrl: './supplier-smart-specialization-card.component.html'
})
export class SupplierSmartSpecializationCardComponent {

  @Input() smartSpecialization: SmartSpecializationGroupDto;
  @Input() supplierId: number;

  constructor(
    public translateService: TranslateService,
    private router: Router,
    private pageHandlingService: PageHandlingService
  ) {
  }

  openReadSupplierOffering(id: number) {
    this.pageHandlingService.scrollToTop();
    this.router.navigate([`/suppliers/${this.supplierId}/offerings/${id}`]);
  }
}
