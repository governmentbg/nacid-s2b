import { Component, Input } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { CompanyAdditionalDto } from "src/companies/dtos/company-additional.dto";

@Component({
    selector: 'company-additional-basic',
    templateUrl: './company-additional-basic.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class CompanyAdditionalBasicComponent {

    @Input() companyAdditionalDto: CompanyAdditionalDto = new CompanyAdditionalDto();
    @Input() isEditMode = false;

}