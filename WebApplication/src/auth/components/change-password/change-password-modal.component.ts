import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { AuthResource } from "src/auth/auth.resource";
import { ssoChangePasswordDto } from "src/auth/dtos/change-password/sso-change-password.dto";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";

@Component({
  selector: 'change-password-modal',
  templateUrl: './change-password-modal.component.html'
})
export class ChangePasswordModalComponent {
  changePasswordDto: ssoChangePasswordDto = new ssoChangePasswordDto();

  constructor(
    private activeModal: NgbActiveModal,
    private authResource: AuthResource,
    public alertMessageService: AlertMessageService
  ) {
  }

  changePassword() {
    if (this.changePasswordDto.newPassword === this.changePasswordDto.newPasswordAgain) {
      this.authResource.changePassword(this.changePasswordDto)
        .subscribe(() => {
          const alertMessage = new AlertMessageDto("successTexts.changePassword", 'fa-solid fa-check-circle', null, 'bg-success text-light', 5000)
          this.alertMessageService.show(alertMessage);
          this.activeModal.close();
        });
    } else {
      const errorAlertMessage =  new AlertMessageDto("errorTexts.User_NewPasswordsMismatch", 'fa-solid fa-triangle-exclamation', null, 'bg-warning text-dark', 5000)
      this.alertMessageService.show(errorAlertMessage);
    }
  }

  close() {
    this.activeModal.close(null);
  }

}