import { Component, OnInit } from "@angular/core";
import { CompanyDto } from "../dtos/company.dto";
import { ActivatedRoute } from "@angular/router";
import { CompanyResource } from "../resources/company.resource";
import { catchError, throwError } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { PermissionService } from "src/auth/services/permission.service";
import { companyAlias, nacidAlias, pniiditAlias } from "src/auth/constants/organizational-unit.constants";
import { companyAdditionalTab } from "../constants/company-details.constants";
import { companyAdditionalReadPermission, companyAdditionalWritePermission } from "src/auth/constants/permission.constants";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { IsActiveDto } from "src/shared/dtos/is-active.dto";
import { CompanyType } from "../enums/company-type.enum";

@Component({
    selector: 'company-details',
    templateUrl: './company-details.component.html',
    styleUrls: ['./company-details.styles.css']
})
export class CompanyDetailsComponent implements OnInit {
    company = new CompanyDto();

    loadingData = false;
    changeIsActivePending = false;

    hasCompanyAdditionalPermission = false;
    hasCompanyAdditionalWritePermission = false;

    activeTab = companyAdditionalTab;
    companyAdditionalTab = companyAdditionalTab;

    type = CompanyType;

    constructor(
        private route: ActivatedRoute,
        private companyResource: CompanyResource,
        private permissionService: PermissionService,
        private modalService: NgbModal
    ) {

    }

    changeIsActive(isActive: boolean) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.text = 'root.modals.changeState';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure';

        return modal.result.then((ok: boolean) => {
            if (ok) {
                this.changeIsActivePending = true;

                var isActiveDto = new IsActiveDto();
                isActiveDto.id = this.company.id;
                isActiveDto.isActive = isActive;

                this.companyResource.changeIsActive(isActiveDto)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.changeIsActivePending = false;
                            return throwError(() => err);
                        })
                    )
                    .subscribe((isActive: boolean) => {
                        this.company.isActive = isActive;
                        this.changeIsActivePending = false;
                    });
            }
        });
    }

    ngOnInit() {
        this.loadingData = true;
        this.route.params.subscribe(p => {
            this.companyResource.getById(p['id'])
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingData = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.company = e;
                    this.hasCompanyAdditionalPermission = this.permissionService.verifyUnitPermission(companyAdditionalReadPermission, [[companyAlias, this.company.id], [nacidAlias, null], [pniiditAlias, null]]);
                    this.hasCompanyAdditionalWritePermission = this.permissionService.verifyUnitPermission(companyAdditionalWritePermission, [[companyAlias, this.company.id], [nacidAlias, null]]);
                    this.loadingData = false;
                });
        });
    }
}