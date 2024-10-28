import { Component, OnInit } from "@angular/core";
import { SamlRequestDto } from "../dtos/saml-request.dto";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { EAuthResource } from "../e-auth.resource";
import { DomSanitizer } from "@angular/platform-browser";
import { catchError, throwError } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
    selector: 'e-auth-send-request-modal',
    templateUrl: './e-auth-send-request-modal.component.html'
})
export class EAuthSendRequestModalComponent implements OnInit {

    samlRequest: SamlRequestDto = null;
    isLoading = false;

    constructor(
        private activeModal: NgbActiveModal,
        private eAuthResource: EAuthResource,
        private sanitizer: DomSanitizer
    ) {
    }

    login() {
        this.sanitizer.bypassSecurityTrustResourceUrl(this.samlRequest.postUrl);
    }

    close() {
        this.activeModal.close(null);
    }

    ngOnInit() {
        this.isLoading = true;
        this.eAuthResource.getSamlRequest()
            .pipe(
                catchError((err: HttpErrorResponse) => {

                    this.isLoading = false;
                    this.close();
                    return throwError(() => new Error('Invalid login.'));
                })
            )
            .subscribe((samlRequest: SamlRequestDto) => {
                this.samlRequest = samlRequest;
                this.isLoading = false;
            });
    }
}