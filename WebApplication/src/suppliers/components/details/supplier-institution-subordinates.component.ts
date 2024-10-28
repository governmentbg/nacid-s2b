import { Component, Input } from "@angular/core";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { OrganizationType } from "src/nomenclatures/enums/organization-type.enum";
import { OwnershipType } from "src/nomenclatures/enums/ownership-type.enum";

@Component({
    selector: 'supplier-institution-subordinates',
    templateUrl: './supplier-institution-subordinates.component.html',
})
export class SupplierInstitutionSubordinatesComponent {
    @Input() subordinates: InstitutionDto[] = [];
    organizationType = OrganizationType;
    ownershipType = OwnershipType;
}
