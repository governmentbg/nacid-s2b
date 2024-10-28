import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { companyAlias, complexAlias, nacidAlias, pniiditAlias } from 'src/auth/constants/organizational-unit.constants';
import { approvalRegistrationReadPermission, commonReportsReadPermission, companyAdditionalReadPermission, nomenclaturesReadPermission, receivedVoucherReadPermission, systemLogReadPermission, voucherRequestReadPermission } from 'src/auth/constants/permission.constants';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordModalComponent } from 'src/auth/components/change-password/change-password-modal.component';
import { PermissionService } from 'src/auth/services/permission.service';
import { UserContextService } from 'src/auth/services/user-context.service';
import { equipmentDetailsTab, offeringDetailsTab, teamDetailsTab } from 'src/suppliers/constants/supplier-details.constants';
import { ReceivedVoucherCountResource } from 'src/received-vouchers/resources/received-voucher-count.resource';
import { Configuration } from 'src/app/configuration/configuration';
import { UserAuthorizationState } from 'src/auth/enums/user-authorization-state.enum';

@Component({
    selector: 'profile-sidebar',
    templateUrl: './profile-sidebar.component.html',
    styleUrls: ['./profile-sidebar.styles.css']
})
export class ProfileSidebarComponent implements OnInit {

    supplierIsCollapsed: boolean[] = [];
    nomenclaturesIsCollapsed = true;
    companyAlias = companyAlias;
    nacidAlias = nacidAlias;
    pniiditAlias = pniiditAlias;
    offeringDetailsTab = offeringDetailsTab;
    teamDetailsTab = teamDetailsTab;
    equipmentDetailsTab = equipmentDetailsTab;
    hasCompanyReadPermission = this.permissionService.verifyUnitPermission(companyAdditionalReadPermission, [[nacidAlias, null], [pniiditAlias, null]]);
    hasSystemLogsReadPermission = this.permissionService.verifyUnitPermission(systemLogReadPermission, [[nacidAlias, null]]);
    hasApproveRegistrationReadPermission = this.permissionService.verifyUnitPermission(approvalRegistrationReadPermission, [[nacidAlias, null]]);
    hasNomenclaturesReadPermission = this.permissionService.verifyUnitPermission(nomenclaturesReadPermission, [[nacidAlias, null]]);
    hasVoucherRequestsReadPermission = this.permissionService.verifyPermission(voucherRequestReadPermission);
    hasReceivedVouchersReadPermission = this.permissionService.verifyPermission(receivedVoucherReadPermission);
    hasCommonReportsReadPermission = this.permissionService.verifyPermission(commonReportsReadPermission);
    receivedVoucherCount: number;

    userAuthorizationState = UserAuthorizationState;

    constructor(
        public offcanvasService: NgbOffcanvas,
        public userContextService: UserContextService,
        public configuration: Configuration,
        private router: Router,
        private permissionService: PermissionService,
        private modalService: NgbModal,
        private receivedVoucherCountResource: ReceivedVoucherCountResource) {
    }

    changeRouter(route: string, id: number = null, tab: string = null) {
        if (id && tab) {
            this.router.navigate([`${route}/${id}/${tab}`]);
        } else if (id) {
            this.router.navigate([route, id]);
        } else {
            this.router.navigate([route]);
        }

        this.offcanvasService.dismiss();
    }

    receivedVoucherRouter() {
        if (!this.configuration.useAllFunctionalities || this.receivedVoucherCount > 0 || !this.userContextService.isCompany()) {
            this.router.navigate(['/receivedVouchers']);
        } else {
            this.router.navigate(['/receivedVouchers/create']);
        }
        this.offcanvasService.dismiss();
    }

    logoutUser() {
        this.userContextService.logout(true);
        this.offcanvasService.dismiss();
    }

    openChangePasswordModal() {
        this.modalService.open(ChangePasswordModalComponent, { backdrop: 'static', size: 'md', keyboard: false });
    }

    ngOnInit() {
        if (this.userContextService.authorizationState === this.userAuthorizationState.login) {
            this.receivedVoucherCountResource.receivedVoucherCount$.subscribe(count => {
                this.receivedVoucherCount = count;
            });

            const suppliersCount = this.userContextService.userContext.organizationalUnits.filter(e => (!e.alias || e.alias === complexAlias) && e.externalId)?.length;

            for (let i = 0; i < suppliersCount; i++) {
                this.supplierIsCollapsed.push(false);
            }
        }
    }
}