import { Component, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SupplierSubordinateGroupDto } from 'src/suppliers/dtos/search/supplier-group/supplier-subordinate-group.dto';

@Component({
  selector: 'supplier-subordinate-card',
  templateUrl: './supplier-subordinate-card.component.html',
  styleUrls: ['./supplier-subordinate-card.styles.css']
})
export class SupplierSubordinateCardComponent {
  @Input() subordinate: SupplierSubordinateGroupDto;

  constructor(
    public translateService: TranslateService,
  ) {
  }
}
