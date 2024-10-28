import { ReportResource } from "src/reports/resources/report.resource";
import { BaseReportSearchComponent } from "../base/base-report-search.component";
import { Component } from "@angular/core";
import { SearchUnsubscriberService } from "src/shared/services/search-unsubscriber/search-unsubscriber.service";
import { OfferingContractReportDto } from "src/reports/dtos/offering-contract-report.dto";
import { OfferingContractReportFilterDto } from "src/reports/filter-dtos/offering-contract-report-filter.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { commonReportsReadPermission } from "src/auth/constants/permission.constants";
import { UserContextService } from "src/auth/services/user-context.service";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { Level } from "src/shared/enums/level.enum";
import { TranslateService } from "@ngx-translate/core";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";

@Component({
    selector: 'offering-contract-report',
    templateUrl: './offering-contract-report.component.html',
    providers: [ReportResource, SearchUnsubscriberService]
})
export class OfferingContractReportComponent extends BaseReportSearchComponent<OfferingContractReportDto, OfferingContractReportFilterDto>{

    supplierType = SupplierType;
    receivedVoucherState = ReceivedVoucherState;
    level = Level;
    isNacidOrPniiditAlias = this.userContextService.isNacid(commonReportsReadPermission) || this.userContextService.isPniidit(commonReportsReadPermission);

    constructor(
        public override pageHandlingService: PageHandlingService,
        protected override resource: ReportResource<OfferingContractReportDto, OfferingContractReportFilterDto>,
        protected override searchUnsubscriberService: SearchUnsubscriberService,
        protected override translateService: TranslateService,
        private readonly userContextService: UserContextService
    ) {
        super(pageHandlingService, resource, searchUnsubscriberService, translateService, OfferingContractReportFilterDto, "offeringContract", "reports.offeringContracts.excelTitle");
    }

    changedInstitution(rootInstitution: InstitutionDto) {
        const changedInstitution = JSON.parse(JSON.stringify(rootInstitution)) as InstitutionDto;
        this.filter.rootInstitution = rootInstitution;
        this.filter.rootInstitutionId = rootInstitution?.id;
        this.filter.institutionId = changedInstitution?.id;
        this.filter.institution = changedInstitution;
    }
}