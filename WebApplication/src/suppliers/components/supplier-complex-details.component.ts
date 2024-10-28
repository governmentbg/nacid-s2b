import { Component, Input } from "@angular/core";
import { ComplexDto } from "src/nomenclatures/dtos/complexes/complex.dto";
import { SupplierRepresentativeDto } from "../dtos/supplier-representative.dto";
import { AreaOfActivity } from "src/nomenclatures/enums/area-of-activity.enum";

@Component({
    selector: 'supplier-complex-details',
    templateUrl: './supplier-complex-details.component.html'
})
export class SupplierComplexDetailsComponent {

    @Input() complex = new ComplexDto();
    @Input() supplierRepresentative = new SupplierRepresentativeDto();

    areaOfActivity = AreaOfActivity;
}