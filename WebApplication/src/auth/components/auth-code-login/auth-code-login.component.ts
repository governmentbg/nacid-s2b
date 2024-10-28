import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { catchError, throwError } from "rxjs";
import { Configuration } from "src/app/configuration/configuration";
import { AuthResource } from "src/auth/auth.resource";
import { LoginWithCodeDto } from "src/auth/dtos/login/login-with-code.dto";
import { TokenResponseDto } from "src/auth/dtos/token/token-response.dto";
import { UserAuthorizationState } from "src/auth/enums/user-authorization-state.enum";
import { UserContextService } from "src/auth/services/user-context.service";

@Component({
    selector: 'auth-code-login',
    template: ''
})
export class AuthCodeLoginComponent implements OnInit {

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private userContextService: UserContextService,
        private authResource: AuthResource,
        private configuration: Configuration
    ) {
    }

    private loginWithCode(authorizationCode: string) {
        if (this.userContextService.authorizationState !== UserAuthorizationState.logout || !authorizationCode) {
            this.clearQueryParams();
            this.router.navigate(['/']);
        } else {
            this.userContextService.authorizationState = UserAuthorizationState.loading;
            var loginWithCodeDto = new LoginWithCodeDto(authorizationCode);

            this.authResource.loginWithCode(loginWithCodeDto)
                .pipe(
                    catchError((err) => {
                        this.clearQueryParams();
                        this.userContextService.logout(true);

                        return throwError(() => err);
                    })
                )
                .subscribe((res: TokenResponseDto) => {
                    this.clearQueryParams();
                    this.userContextService.setToken(res, this.configuration.hosting, true);
                });
        }
    }

    private clearQueryParams() {
        const queryParams: Params = { code: null };

        this.router.navigate(
            [],
            {
                relativeTo: this.activatedRoute,
                queryParams: queryParams
            });
    }

    ngOnInit() {
        const authorizationCode = this.activatedRoute.snapshot.queryParams['code'];
        this.loginWithCode(authorizationCode);
    }
}