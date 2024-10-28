import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UserAuthorizationState } from 'src/auth/enums/user-authorization-state.enum';
import { UserContextService } from 'src/auth/services/user-context.service';

@Injectable()
export class LogoutAuthGuard {

    authorizationState = UserAuthorizationState;

    constructor(
        public router: Router,
        private userContextService: UserContextService
    ) { }

    canActivate(): boolean {
        if (this.userContextService.authorizationState !== this.authorizationState.logout
            && this.userContextService.authorizationState !== this.authorizationState.eAuthLogin) {
            this.router.navigate(['/']);
            return false;
        } else {
            return true;
        }
    }
}
