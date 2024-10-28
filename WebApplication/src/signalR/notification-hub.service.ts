import { Injectable } from "@angular/core";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { NotificationDto } from "src/signalR/notification/dtos/notification.dto";

@Injectable()
export class NotificationHubService {

    hubConnectionBuilder: HubConnection;

    notifications: NotificationDto[] = [];

    connectHub(hosting: string, userId: number) {
        this.disconnectHub();

        this.hubConnectionBuilder = new HubConnectionBuilder()
            .withUrl(`${hosting}notificationHub?userId=${userId}`)
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        this.hubConnectionBuilder
            .start()
            .then(() => { });
    }

    disconnectHub() {
        if (this.hubConnectionBuilder) {
            this.hubConnectionBuilder.stop();
        }
    }
}