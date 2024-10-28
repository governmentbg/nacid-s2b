import { Component, OnInit } from "@angular/core";
import { NotificationHubService } from "../../notification-hub.service";
import { NotificationDto } from "src/signalR/notification/dtos/notification.dto";
import { Router } from "@angular/router";
import { PageHandlingService } from "src/shared/services/page-handling/page-handling.service";
import { NotificationEntityType } from "../enums/notification-entity-type.enum";
import { NotificationType } from "../enums/notification-type.enum";
import { NotificationResource } from "../resources/notification.resource";

@Component({
    selector: 'notification-container',
    templateUrl: 'notification-container.component.html',
    host: { class: 'toast-container position-fixed bottom-0 end-0 p-3 cursor-pointer', style: 'z-index: 9999' },
})
export class NotificationContainerComponent implements OnInit {

    notificationType = NotificationType;

    currentRouter = '';

    constructor(
        public notificationHubService: NotificationHubService,
        private router: Router,
        private pageHandlingService: PageHandlingService,
        private notificationResource: NotificationResource) {
    }

    close(index: number) {
        this.notificationHubService.notifications.splice(index, 1);
    }

    getEntity(entityId: number, entityType: NotificationEntityType, index: number) {
        this.pageHandlingService.scrollToTop();
        this.router.navigate([entityType === NotificationEntityType.voucherRequest ? '/voucherRequests' : '/receivedVouchers', entityId]);
        this.close(index);
    }

    ngOnInit() {
        this.notificationHubService.hubConnectionBuilder.on('SendNotification', (recievedNotification: NotificationDto) => {
            if (recievedNotification.entityType === NotificationEntityType.voucherRequest
                && (this.router.url === `/voucherRequests/${recievedNotification.entityId}` || this.router.url === `/suppliers/${recievedNotification.supplierId}/offerings/${recievedNotification.offeringId}`)) {
                this.notificationResource.deleteVrNotification(recievedNotification.entityId).subscribe(() => { });
            } else if (recievedNotification.entityType === NotificationEntityType.receivedVoucher && this.router.url === `/receivedVouchers/${recievedNotification.entityId}`) {
                this.notificationResource.deleteRvNotification(recievedNotification.entityId).subscribe(() => { });
            } else {
                this.notificationHubService.notifications.push(recievedNotification);
            }
        });
    }
}