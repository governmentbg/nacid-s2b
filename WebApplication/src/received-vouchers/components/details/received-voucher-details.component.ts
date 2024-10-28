import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { companyAlias, nacidAlias, pniiditAlias } from "src/auth/constants/organizational-unit.constants";
import { receivedVoucherDeletePermission, receivedVoucherWritePermission } from "src/auth/constants/permission.constants";
import { PermissionService } from "src/auth/services/permission.service";
import { ReceivedVoucherDto } from "src/received-vouchers/dtos/received-voucher.dto";
import { ReceivedVoucherState } from "src/received-vouchers/enums/received-voucher-state.enum";
import { ReceivedVoucherVerifyStateModalComponent } from "./modals/received-voucher-verify-state-modal.component";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { ReceivedVoucherPermissionService } from "src/received-vouchers/services/received-voucher-permission.service";
import { ReceivedVoucherResource } from "src/received-vouchers/resources/received-voucher.resource";
import { HistoryReceivedVoucherDetailsComponent } from "./history-received-voucher-details.component";
import { ReceivedVoucherCertificateDto } from "src/received-vouchers/dtos/received-voucher-certificate.dto";
import { ReceivedVoucherCertificateModalComponent } from "./modals/received-voucher-certificate-modal.component";
import { TerminateReceivedVoucherModal } from "./modals/received-voucher-terminate-modal.component";
import { NotificationResource } from "src/signalR/notification/resources/notification.resource";
import { NotificationHubService } from "src/signalR/notification-hub.service";
import { NotificationDto } from "src/signalR/notification/dtos/notification.dto";
import { NotificationEntityType } from "src/signalR/notification/enums/notification-entity-type.enum";
import { NotificationType } from "src/signalR/notification/enums/notification-type.enum";
import { UserContextService } from "src/auth/services/user-context.service";

@Component({
    selector: 'received-voucher-details',
    templateUrl: './received-voucher-details.component.html'
})
export class ReceivedVoucherDetailsComponent implements OnInit {

    loadingData = false;
    terminateDataPending = false;
    editReceivedVoucherPending = false;
    isEditMode = false;

    canReadCommunication = false;
    hasEditDataPermissions = false;
    canGenerateCertificate = false;
    hasDeleteDataPermission = false;

    receivedVoucher = new ReceivedVoucherDto();
    originalModel = new ReceivedVoucherDto();

    receivedVoucherState = ReceivedVoucherState;
    notificationType = NotificationType;

    @ViewChild(NgForm) form: NgForm;

    constructor(
        public router: Router,
        private route: ActivatedRoute,
        private resource: ReceivedVoucherResource,
        private permissionService: PermissionService,
        private receivedVoucherPermissionService: ReceivedVoucherPermissionService,
        private modalService: NgbModal,
        private notificationResource: NotificationResource,
        private notificationHubService: NotificationHubService,
        private userContextService: UserContextService
    ) { }

    edit() {
        this.originalModel = JSON.parse(JSON.stringify(this.receivedVoucher)) as ReceivedVoucherDto;
        this.isEditMode = true;
    }

    cancel() {
        this.receivedVoucher = JSON.parse(JSON.stringify(this.originalModel)) as ReceivedVoucherDto;
        this.isEditMode = false;
        this.originalModel = null;
    }

    save() {
        if (this.form.valid) {
            if (this.receivedVoucher.file != null && this.receivedVoucher.supplierId != null && this.receivedVoucher.offeringId != null && this.receivedVoucher.receivedOffering != null) { 
                const modal = this.modalService.open(ReceivedVoucherVerifyStateModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });

                modal.result.then((state: ReceivedVoucherState) => {
                    if (state) {
                        if (state === ReceivedVoucherState.completed) {
                            this.updateData(ReceivedVoucherState.completed);
                        } else if (state === ReceivedVoucherState.draft) {
                            this.updateData(ReceivedVoucherState.draft);
                        }
                    }
                });
            } else {
                const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
                modal.componentInstance.text = 'receivedVouchers.modals.draftTitle';
                modal.componentInstance.acceptButton = 'root.buttons.yesSure';

                modal.result.then((ok: boolean) => {
                    if (ok) {
                        this.updateData(ReceivedVoucherState.draft);
                    }
                });
            }
        }
    }

    openHistory() {
        const modal = this.modalService.open(HistoryReceivedVoucherDetailsComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.filter.receivedVoucherId = this.receivedVoucher.id
    }


    openTerminateVoucherModal() {
        const modal = this.modalService.open(TerminateReceivedVoucherModal, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.receivedVoucher = this.receivedVoucher;

        modal.result.then((updatedVoucher: ReceivedVoucherDto) => {
            if (updatedVoucher) {
                this.receivedVoucher = updatedVoucher;
            }
        });
    }

    openCertificateGeneration() {
        const modal = this.modalService.open(ReceivedVoucherCertificateModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.receivedVoucherDto = this.receivedVoucher;

        modal.result.then((receivedVoucherCertificateDto: ReceivedVoucherCertificateDto) => {
            if (receivedVoucherCertificateDto) {
                this.receivedVoucher.certificates.push(receivedVoucherCertificateDto);
                this.canGenerateCertificate = this.receivedVoucherPermissionService.canGenerateCertificate(this.receivedVoucher);

                if (this.receivedVoucher.state === this.receivedVoucherState.completed) {
                    this.receivedVoucher.state = this.receivedVoucherState.generatedCertificate;
                }
            }
        });
    }

    private updateData(state: ReceivedVoucherState) {
        this.editReceivedVoucherPending = true;
        this.receivedVoucher.state = state;
        this.resource
            .update(this.receivedVoucher)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.editReceivedVoucherPending = false;
                    this.isEditMode = false;
                    this.originalModel = null;
                    return throwError(() => err);
                })
            )
            .subscribe(() => {
                this.editReceivedVoucherPending = false;
                this.isEditMode = false;
                this.originalModel = null;
            });
    }

    ngOnInit() {
        this.loadingData = true;
        this.route.params.subscribe(p => {
            this.resource.getById(p['id'])
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingData = false;
                        this.router.navigate(['/receivedVouchers']);
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.receivedVoucher = e;

                    this.notificationResource.deleteRvNotification(e.id).subscribe(() => { });

                    this.canReadCommunication = this.userContextService.isSupplier() || this.userContextService.isCompany();
                    this.hasEditDataPermissions = this.permissionService
                        .verifyUnitPermission(receivedVoucherWritePermission, [[companyAlias, e.companyId]]);
                    this.canGenerateCertificate = this.receivedVoucherPermissionService.canGenerateCertificate(e);

                    this.hasDeleteDataPermission = this.permissionService.verifyUnitPermission(receivedVoucherDeletePermission, [[companyAlias, e.companyId], [nacidAlias, null], [pniiditAlias, null]])
                    this.loadingData = false;
                });
        });

        this.notificationHubService.hubConnectionBuilder.on('SendNotification', (recievedNotification: NotificationDto) => {
            if (recievedNotification.entityType === NotificationEntityType.receivedVoucher && this.router.url === `/receivedVouchers/${recievedNotification.entityId}`) {
                if (recievedNotification.type === this.notificationType.changedState) {
                    this.receivedVoucher.state = recievedNotification.rvState;
                }
            }
        });
    }
}