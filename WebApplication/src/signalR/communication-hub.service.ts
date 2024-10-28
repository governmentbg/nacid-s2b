import { Injectable } from "@angular/core";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { Configuration } from "src/app/configuration/configuration";

@Injectable()
export class CommunicationHubService {

    hubConnectionBuilder: HubConnection;
    private groupName: string;

    constructor(private configuration: Configuration) {
    }

    connectHub(hubName: string, groupName: string) {
        this.disconnectHub();

        this.groupName = groupName;

        this.hubConnectionBuilder = new HubConnectionBuilder()
            .withUrl(`${this.configuration.hosting}${hubName}`)
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        this.hubConnectionBuilder
            .start()
            .then(() => this.joinGroup());
    }

    leaveGroup() {
        this.hubConnectionBuilder.invoke("LeaveGroup", this.groupName);
    }

    private joinGroup() {
        this.hubConnectionBuilder.invoke("JoinGroup", this.groupName);
    }

    private disconnectHub() {
        if (this.hubConnectionBuilder) {
            this.hubConnectionBuilder.stop();
        }
    }
}