import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { NotificationDto } from "../dtos/notification.dto";

@Injectable()
export class NotificationResource {

    url = 'api/notifications';

    constructor(
        private http: HttpClient
    ) {
    }

    getNotifications(): Observable<NotificationDto[]> {
        return this.http.get<NotificationDto[]>(`${this.url}`);
    }

    deleteVrNotification(voucherRequestId: number): Observable<void> {
        return this.http.delete<void>(`${this.url}/voucherRequests/${voucherRequestId}`);
    }

    deleteRvNotification(receivedVoucherId: number): Observable<void> {
        return this.http.delete<void>(`${this.url}/receivedVouchers/${receivedVoucherId}`);
    }
}