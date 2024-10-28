import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { UserContextService } from "src/auth/services/user-context.service";
import { EAuthResponseStatus } from "../enums/e-auth-response-status.enum";

@Component({
    selector: 'e-auth-response',
    templateUrl: './e-auth-response.component.html'
})
export class EAuthResponseComponent implements OnInit {

    responseStatus: any;
    errorMessage: string;
    loading = true;

    constructor(
        private route: ActivatedRoute,
        private userContextService: UserContextService
    ) {
    }

    setErrorMessage() {
        if (this.responseStatus === EAuthResponseStatus.AuthenticationFailed) {
            this.errorMessage = 'Проблем при автентикацията';
        }
        if (this.responseStatus === EAuthResponseStatus.CanceledByUser) {
            this.errorMessage = 'Процеса е прекъснат от потребител';
        }
        if (this.responseStatus === EAuthResponseStatus.InvalidResponseXML) {
            this.errorMessage = 'Грешка в генерирания XML';
        }
        if (this.responseStatus === EAuthResponseStatus.InvalidSignature) {
            this.errorMessage = 'Невалиден подпис';
        }
        if (this.responseStatus === EAuthResponseStatus.MissingEGN) {
            this.errorMessage = 'Липсва ЕГН';
        }
        if (this.responseStatus === EAuthResponseStatus.NotDetectedQES) {
            this.errorMessage = 'Подписът не е КЕП';
        }

        this.loading = false;
    }

    ngOnInit() {
        this.responseStatus = EAuthResponseStatus[this.route.snapshot.queryParams['responseStatus']];

        if (this.responseStatus === EAuthResponseStatus.Success) {
            this.loading = false;
            this.userContextService.loginEAuth(this.route.snapshot.queryParams['name']);
        } else {
            this.setErrorMessage();
        }
    }
}