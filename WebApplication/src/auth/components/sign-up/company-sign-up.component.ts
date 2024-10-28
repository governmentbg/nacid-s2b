import { HttpErrorResponse } from "@angular/common/http";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { catchError, throwError } from "rxjs";
import { CompanyDto } from "src/companies/dtos/company.dto";
import { CompanyType } from "src/companies/enums/company-type.enum";
import { AgencyRegixResource } from "src/shared/regix/agency-regix.resource";
import { SettlementChangeService } from "src/shared/services/settlement-change/settlement-change.service";

@Component({
    selector: 'company-sign-up',
    templateUrl: './company-sign-up.component.html',
    providers: [SettlementChangeService],
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class CompanySignUpComponent {

    @Output() companyChanged = new EventEmitter<CompanyDto>();

    companyType = CompanyType;
    getCompanyByUicPending = false;

    company: CompanyDto;
    @Input('company')
    set companySetter(company: CompanyDto) {
        this.company = company;
        this.company.isRegistryAgency = true;
    }

    constructor(
        private agencyRegixResource: AgencyRegixResource,
        public settlementChangeService: SettlementChangeService,
        public translateService: TranslateService) {
    }

    registryAgencyChanged() {
        this.company.lawForm = null;
        this.company.lawFormId = null;
        this.company.uic = null;
        this.companyChanged.emit(this.company);
    }

    uicChanged() {
        this.company.lawForm = null;
        this.company.lawFormId = null;
        this.companyChanged.emit(this.company);
    }

    getCompanyByUic() {
        this.getCompanyByUicPending = true;
        this.agencyRegixResource
            .getCompanyFromAgencyRegix(this.company.uic)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.company.lawForm = null;
                    this.company.lawFormId = null;
                    this.companyChanged.emit(this.company);
                    this.getCompanyByUicPending = false;
                    return throwError(() => err);
                })
            )
            .subscribe((e: CompanyDto) => {
                this.company = e;
                this.companyChanged.emit(this.company);
                this.getCompanyByUicPending = false;
            });
    }
}