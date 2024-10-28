import { Component, OnInit } from "@angular/core";
import { VoucherRequestDto } from "../dtos/voucher-request.dto";
import { ActivatedRoute, Router } from "@angular/router";
import { VoucherRequestResource } from "../resources/voucher-request.resource";
import { catchError, throwError } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { VoucherRequestState } from "../enums/voucher-request-state.enum";
import { PermissionService } from "src/auth/services/permission.service";
import { voucherRequestWritePermission } from "src/auth/constants/permission.constants";
import { companyAlias, complexAlias } from "src/auth/constants/organizational-unit.constants";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { VoucherRequestStateDto } from "../dtos/voucher-request-state.dto";
import { NotificationHubService } from "src/signalR/notification-hub.service";
import { NotificationDto } from "../../signalR/notification/dtos/notification.dto";
import { UserContextService } from "src/auth/services/user-context.service";
import { NotificationEntityType } from "src/signalR/notification/enums/notification-entity-type.enum";
import { NotificationType } from "src/signalR/notification/enums/notification-type.enum";
import { NotificationResource } from "src/signalR/notification/resources/notification.resource";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { VoucherRequestDeclineCodeComponent } from "./modals/voucher-request-decline-code-modal.component";

@Component({
    selector: 'voucher-request-details',
    templateUrl: './voucher-request-details.component.html'
})
export class VoucherRequestDetailsComponent implements OnInit {

    hasVoucherRequestCodePermission = false;
    hasVoucherReuqestGeneratePermission = false;
    canReadCommunication = false;
    requestCodePending = false;
    generateCodePending = false;
    declineCodePending = false;

    loadingData = false;

    voucherRequest = new VoucherRequestDto();

    voucherRequestState = VoucherRequestState;
    supplierType = SupplierType;
    notificationType = NotificationType;

    constructor(
        public router: Router,
        private route: ActivatedRoute,
        private resource: VoucherRequestResource,
        private permissionService: PermissionService,
        private notificationResource: NotificationResource,
        private notificationHubService: NotificationHubService,
        private userContextService: UserContextService,
        private modalService: NgbModal,

    ) { }

    requestCode() {
        this.requestCodePending = true;

        const voucherRequestStateDto = new VoucherRequestStateDto();
        voucherRequestStateDto.requestCompanyId = this.voucherRequest.requestCompanyId;
        voucherRequestStateDto.supplierOfferingId = this.voucherRequest.supplierOfferingId;
        voucherRequestStateDto.state = this.voucherRequestState.requested;

        this.resource
            .requestCode(voucherRequestStateDto)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.requestCodePending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.requestCodePending = false;
                this.voucherRequest.state = e.state;
            });
    }

    generateCode() {
        this.generateCodePending = true;

        const voucherRequestStateDto = new VoucherRequestStateDto();
        voucherRequestStateDto.requestCompanyId = this.voucherRequest.requestCompanyId;
        voucherRequestStateDto.supplierOfferingId = this.voucherRequest.supplierOfferingId;
        voucherRequestStateDto.state = this.voucherRequestState.generated;

        this.resource
            .generateCode(voucherRequestStateDto)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.generateCodePending = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.generateCodePending = false;
                this.voucherRequest.state = e.state;
                this.voucherRequest.code = e.code;
            });
    }

    openDeclineCodeModal() {
        const modal = this.modalService.open(VoucherRequestDeclineCodeComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.state = this.voucherRequestState.declined;
        modal.componentInstance.voucherRequest = this.voucherRequest;
    }

    ngOnInit() {
        this.loadingData = true;
        this.route.params.subscribe(p => {
            this.resource.getById(p['id'])
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingData = false;
                        this.router.navigate(['/voucherRequests']);
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.voucherRequest = e;

                    this.notificationResource.deleteVrNotification(e.id).subscribe(() => { });

                    this.canReadCommunication = this.userContextService.isSupplier() || this.userContextService.isCompany();
                    this.hasVoucherRequestCodePermission = this.permissionService
                        .verifyUnitPermission(voucherRequestWritePermission, [[companyAlias, e.requestCompanyId]]);
                    this.hasVoucherReuqestGeneratePermission = this.permissionService
                        .verifyUnitPermission(voucherRequestWritePermission, [[e.supplierOffering.supplier.type === this.supplierType.institution ? null : complexAlias, e.supplierOffering.supplier.type === this.supplierType.institution ? e.supplierOffering.supplier.institutionId : e.supplierOffering.supplier.complexId]]);
                    this.loadingData = false;
                });
        });

        this.notificationHubService.hubConnectionBuilder.on('SendNotification', (recievedNotification: NotificationDto) => {
            if (recievedNotification.entityType === NotificationEntityType.voucherRequest && this.router.url === `/voucherRequests/${recievedNotification.entityId}`) {
                if (recievedNotification.type === this.notificationType.changedState) {
                    this.voucherRequest.state = recievedNotification.vrState;
                } else if (recievedNotification.type === this.notificationType.generatedCode) {
                    this.voucherRequest.code = recievedNotification.code;
                }
            }
        });
    }
}