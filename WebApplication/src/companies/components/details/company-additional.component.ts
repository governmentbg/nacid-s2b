import { HttpErrorResponse } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { companyAlias, nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { companyAdditionalCreatePermission, companyAdditionalWritePermission } from "src/auth/constants/permission.constants";
import { PermissionService } from "src/auth/services/permission.service";
import { CompanyAdditionalDto } from "src/companies/dtos/company-additional.dto";
import { CompanyAdditionalResource } from "src/companies/resources/company-additional.resource";
import { CompanyAdditionalAddModalComponent } from "./company-additionals/modals/company-additional-add-modal.component";
import { CompanyAdditionalEditModalComponent } from "./company-additionals/modals/company-additional-edit-modal.component";
import { Configuration } from "src/app/configuration/configuration";

@Component({
    selector: 'company-additional',
    templateUrl: './company-additional.component.html',
})
export class CompanyAdditionalComponent {

    hasCompanyAdditionalCreatePermission = false;
    hasCompanyAdditionalWritePermission = false;

    companyAdditional: CompanyAdditionalDto = null;

    loadingData = false;

    companyId: number;
    @Input('companyId')
    set companyIdSetter(companyId: number) {
        this.loadingData = true;
        this.companyId = companyId;
        this.hasCompanyAdditionalCreatePermission = this.permissionService.verifyUnitPermission(companyAdditionalCreatePermission, [[companyAlias, companyId], [nacidAlias, null]]);
        this.hasCompanyAdditionalWritePermission = this.permissionService.verifyUnitPermission(companyAdditionalWritePermission, [[companyAlias, companyId], [nacidAlias, null]]);
        this.getCompanyAdditional(companyId);
    }

    @Input() companyIsActive: boolean;

    constructor(
        private resource: CompanyAdditionalResource,
        private permissionService: PermissionService,
        private modalService: NgbModal,
        public configuration: Configuration
    ) {
    }

    openAddCompanyAdditional() {
        const modal = this.modalService.open(CompanyAdditionalAddModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.companyId = this.companyId;

        return modal.result.then((newCompanyAdditionalDto: CompanyAdditionalDto) => {
            if (newCompanyAdditionalDto) {
                this.companyAdditional = newCompanyAdditionalDto;
            }
        });
    }

    openEditCompanyAdditional() {
        const modal = this.modalService.open(CompanyAdditionalEditModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.companyId = this.companyId;

        return modal.result.then((updatedCompanyAdditionalDto: CompanyAdditionalDto) => {
            if (updatedCompanyAdditionalDto) {
                this.companyAdditional = updatedCompanyAdditionalDto;
            }
        });
    }

    private getCompanyAdditional(companyId: number) {
        this.resource
            .getById(companyId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.companyAdditional = e;
                this.loadingData = false;
            });
    }
}