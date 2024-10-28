import { Component, Input } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { SignUpDto } from "src/auth/dtos/sign-up/sign-up.dto";
import { SignUpType } from "src/auth/enums/sign-up-type.enum";
import { SupplierExtendedType } from "src/auth/enums/supplier-extended-type.enum";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { OrganizationType } from "src/nomenclatures/enums/organization-type.enum";
import { OwnershipType } from "src/nomenclatures/enums/ownership-type.enum";
import { Level } from "src/shared/enums/level.enum";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";

@Component({
    selector: 'supplier-sign-up',
    templateUrl: './supplier-sign-up.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class SupplierSignUpComponent {

    organizationType = OrganizationType;
    signUpType = SignUpType;
    supplierType = SupplierType;
    supplierExtendedType = SupplierExtendedType;
    ownershipType = OwnershipType;
    level = Level;
    rootUniversityTypes: OrganizationType[] = [this.organizationType.university, this.organizationType.specializedUniversity, this.organizationType.independentCollege];
    rootScientificOrganizationTypes: OrganizationType[] = [this.organizationType.scientificOrganization];

    @Input() signUp: SignUpDto;
    @Input() readonly = false;

    constructor(public translateService: TranslateService) {
    }

    changedInstitution(institution: InstitutionDto) {
        const changedInstitution = JSON.parse(JSON.stringify(institution)) as InstitutionDto;
        this.signUp.institution = changedInstitution;
    }

    changeSupplierExtendedType(selectedSupplierExtendedType: SupplierExtendedType) {
        if (selectedSupplierExtendedType === this.supplierExtendedType.university || selectedSupplierExtendedType === this.supplierExtendedType.scientificOrganization) {
            this.signUp.supplierType = this.supplierType.institution;
        } else {
            this.signUp.supplierType = this.supplierType.complex;
        }

        this.signUp.supplierExtendedType = selectedSupplierExtendedType;
        this.signUp.institution = null;
        this.signUp.rootInstitution = null;
        this.signUp.complex = null;
    }
}