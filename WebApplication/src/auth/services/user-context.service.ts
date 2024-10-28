import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, Observable, Observer, Subject, throwError } from "rxjs";
import { AuthResource } from "../auth.resource";
import { accessToken } from "../constants/auth.constants";
import { TokenResponseDto } from "../dtos/token/token-response.dto";
import { UserContext } from "../dtos/user-context.dto";
import { UserAuthorizationState } from "../enums/user-authorization-state.enum";
import { approvalRegistrationReadPermission } from "../constants/permission.constants";
import { companyAlias, complexAlias, nacidAlias, pniiditAlias } from "../constants/organizational-unit.constants";
import { NotificationHubService } from "src/signalR/notification-hub.service";
import { NotificationResource } from "src/signalR/notification/resources/notification.resource";
import { ReceivedVoucherCountResource } from "src/received-vouchers/resources/received-voucher-count.resource";

@Injectable()
export class UserContextService {
    userContext: UserContext;
    authorizationState: UserAuthorizationState = UserAuthorizationState.loading;
    logoutSubject = new Subject<void>();

    constructor(
        private router: Router,
        private authResource: AuthResource,
        private notificationHubService: NotificationHubService,
        private notificationResource: NotificationResource,
        private receivedVoucherCountResource: ReceivedVoucherCountResource
    ) {
    }

    setToken(tokenResponseDto: TokenResponseDto, hosting: string, redirectAfterLogin: boolean) {
        localStorage.setItem(accessToken, tokenResponseDto.access_token);

        return this.getUserInfo(tokenResponseDto.clientId, hosting).subscribe(() => {
            if (redirectAfterLogin) {
                if (this.isNacid(approvalRegistrationReadPermission)) {
                    this.router.navigate(['/approveRegistrations']);
                } else if (this.isCompany()) {
                    this.router.navigate(['/']);
                } else if (this.isSupplier()) {
                    // let supplierId = this.userContext.organizationalUnits.find(unit => unit.externalId && (unit.alias === complexAlias || !unit.alias))?.supplierId;
                    // this.router.navigate([`/suppliers/${supplierId}`]);
                    this.router.navigate(['/voucherRequests']);
                } else {
                    this.router.navigate(['/']);
                }
            }
        });
    }

    logout(redirect: boolean) {
        this.authorizationState = UserAuthorizationState.logout;
        this.notificationHubService.disconnectHub();
        this.userContext = new UserContext();
        localStorage.clear();
        this.logoutSubject.next();

        if (redirect) {
            this.router.navigate(['/']);
        }
    }

    loginEAuth(fullName: string) {
        this.authorizationState = UserAuthorizationState.eAuthLogin;
        this.userContext = new UserContext();
        this.userContext.fullName = fullName;
        localStorage.clear();
        this.router.navigate(['/']);
    }

    getUserInfo(scClientId: string, hosting: string) {
        this.authorizationState = UserAuthorizationState.loading;
        return new Observable((observer: Observer<any>) => {
            return this.authResource.getUserInfo()
                .pipe(
                    catchError((err) => {
                        this.userContext = new UserContext();
                        this.authorizationState = UserAuthorizationState.logout;
                        observer.next(this.userContext);
                        observer.complete();
                        return throwError(() => err);
                    })
                )
                .subscribe(userContext => {
                    this.userContext = userContext;
                    this.authorizationState = (userContext?.userId && scClientId === userContext?.clientId) ? UserAuthorizationState.login : UserAuthorizationState.logout;

                    if (this.authorizationState === UserAuthorizationState.login) {
                        this.notificationHubService.connectHub(hosting, this.userContext.userId);
                        this.notificationResource.getNotifications()
                            .subscribe(e => {
                                this.notificationHubService.notifications = this.notificationHubService.notifications.concat(e)
                            });
                        if (this.isCompany()) {
                            this.receivedVoucherCountResource.fetchVoucherCountForCompany();
                        }
                    }

                    observer.next(this.userContext);
                    observer.complete();
                });
        });
    }

    isPniidit(permission: string): boolean {
        return this.userContext?.organizationalUnits?.some(unit =>
            unit.alias === pniiditAlias && (!permission || unit.permissions.includes(permission))
        )
    }

    isNacid(permission: string): boolean {
        return this.userContext?.organizationalUnits?.some(unit =>
            unit.alias === nacidAlias && (!permission || unit.permissions.includes(permission))
        )
    }

    isCompany(): boolean {
        return this.userContext?.organizationalUnits?.some(unit =>
            unit.alias === companyAlias
        )
    }

    isSupplier(): boolean {
        return this.userContext?.organizationalUnits?.some(unit =>
            unit.externalId
            && (unit.alias === complexAlias || !unit.alias)
        )
    }
}
