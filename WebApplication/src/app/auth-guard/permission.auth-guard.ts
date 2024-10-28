import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router } from "@angular/router";
import { UserAuthorizationState } from "src/auth/enums/user-authorization-state.enum";
import { PermissionService } from "src/auth/services/permission.service";
import { UserContextService } from "src/auth/services/user-context.service";

@Injectable()
export class PermissionAuthGuard {

    authorizationState = UserAuthorizationState;

    constructor(
        public router: Router,
        private userContextService: UserContextService,
        private permissionService: PermissionService
    ) { }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        if (this.userContextService.authorizationState !== this.authorizationState.login) {
            this.router.navigate(['/']);
            return false;
        } else {
            let permission = route.data['permission'] as string;
            let unitExternals = route.data['unitExternals'] as Array<[string, string]>;

            let unitExternalIds: Array<[string, number]> = [];

            if (unitExternals) {
                unitExternals.forEach(e => {
                    let unitExternalId: [string, number] = [e[0], e[1] ? +route.paramMap.get(e[1]) : null];
                    unitExternalIds.push(unitExternalId);
                });
            }

            if (unitExternalIds?.length > 0
                ? this.permissionService.verifyUnitPermission(permission, unitExternalIds)
                : this.permissionService.verifyPermission(permission)) {
                return true;
            } else {
                this.router.navigate(['/']);
                return false;
            }
        }
    }
}