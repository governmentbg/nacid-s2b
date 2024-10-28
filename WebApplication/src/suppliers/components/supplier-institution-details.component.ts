import { Component, Input } from "@angular/core";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { OrganizationType } from "src/nomenclatures/enums/organization-type.enum";
import { SupplierRepresentativeDto } from "../dtos/supplier-representative.dto";
import { OwnershipType } from "src/nomenclatures/enums/ownership-type.enum";

@Component({
    selector: 'supplier-institution-details',
    templateUrl: './supplier-institution-details.component.html'
})
export class SupplierInstitutionDetailsComponent {

    @Input() institution = new InstitutionDto();
    @Input() supplierRepresentative = new SupplierRepresentativeDto();

    organizationType = OrganizationType;
    ownershipType = OwnershipType;
}