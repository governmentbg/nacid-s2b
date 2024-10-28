import { Component, Input } from "@angular/core";
import { SupplierRepresentativeDto } from "src/suppliers/dtos/supplier-representative.dto";

@Component({
    selector: 'supplier-representative',
    templateUrl: './supplier-representative.component.html',
})
export class SupplierRepresentativeComponent {

    @Input() supplierRepresentative: SupplierRepresentativeDto;

}