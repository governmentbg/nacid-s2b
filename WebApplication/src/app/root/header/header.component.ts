import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { LoginModalComponent } from 'src/auth/components/login/login-modal.component';
import { ProfileSidebarComponent } from 'src/app/root/profile-sidebar/profile-sidebar.component';
import { TokenResponseDto } from 'src/auth/dtos/token/token-response.dto';
import { UserAuthorizationState } from 'src/auth/enums/user-authorization-state.enum';
import { UserContextService } from 'src/auth/services/user-context.service';
import { nacidAlias } from 'src/auth/constants/organizational-unit.constants';
import { approvalRegistrationReadPermission } from 'src/auth/constants/permission.constants';
import { PermissionService } from 'src/auth/services/permission.service';
import { Configuration } from 'src/app/configuration/configuration';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.styles.css']
})
export class HeaderComponent implements OnInit {

    authorizationState = UserAuthorizationState;
    hasCompaniesPermission = false;
    nacidAlias = nacidAlias;
    hasApproveRegistrationReadPermission = false;

    @ViewChild('toggleBtn', { static: true }) toggleBtn: ElementRef;
    @ViewChild("collapseContainer", { static: false }) collapseContainer: ElementRef;

    @HostListener('document:click', ['$event']) onClickOutside(): void {
        if (this.collapseContainer
            && this.collapseContainer.nativeElement.classList.contains('show')) {
            this.toggleBtn.nativeElement.click();
        }
    }

    constructor(
        public userContextService: UserContextService,
        private modalService: NgbModal,
        private offcanvasService: NgbOffcanvas,
        private permissionService: PermissionService,
        private configuration: Configuration) {

    }

    checkPermissions(): void {
        this.hasApproveRegistrationReadPermission = this.permissionService.verifyUnitPermission(approvalRegistrationReadPermission, [[nacidAlias, null]]
        );
    }


    openLoginModal() {
        const modal = this.modalService.open(LoginModalComponent, { size: 'md', keyboard: false });

        return modal.result.then((tokenRes: TokenResponseDto) => {
            if (tokenRes && tokenRes.access_token) {
                this.userContextService.setToken(tokenRes, this.configuration.hosting, true);
            }
        });
    }

    openProfileSidebar() {
        this.offcanvasService.open(ProfileSidebarComponent, { position: 'end' });
    }

    ngOnInit() {
        this.checkPermissions();
        this.userContextService.logoutSubject.subscribe(() => {
            this.checkPermissions();
        });
    }
}
