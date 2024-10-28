import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, Observable, throwError } from "rxjs";
import { UserContextService } from "src/auth/services/user-context.service";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { DomainErrorMessageDto } from "src/shared/components/alert-message/models/domain-error-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";
import { LoadingSectionService } from "src/shared/components/loading-section/loading-section.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private userContextService: UserContextService,
        private loadingSectionService: LoadingSectionService,
        private alertMessageService: AlertMessageService
    ) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next
            .handle(request)
            .pipe(
                catchError((err) => {
                    this.loadingSectionService.next(false);

                    if (err.status === 422) {
                        let error = err.error as DomainErrorMessageDto;
                        const alertMessage = new AlertMessageDto('errorTexts.' + error.errorCode, 'fa-solid fa-triangle-exclamation', error.errorText, 'bg-danger text-light');
                        this.alertMessageService.show(alertMessage);
                    } else if (err.status >= 500 && err.status <= 502) {
                        const alertMessage = new AlertMessageDto('errorTexts.System_InternalServerError');
                        this.alertMessageService.show(alertMessage);
                    } else if (err.status === 503) {
                        const alertMessage = new AlertMessageDto('errorTexts.System_ServerUnavailible');
                        this.alertMessageService.show(alertMessage);
                    } else if (err.status === 401) {
                        this.userContextService.logout(true);
                    }

                    return throwError(() => err);
                })
            );
    }
}