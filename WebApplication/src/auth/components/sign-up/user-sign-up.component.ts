import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { ApproveRegistrationFileDto } from "src/approve-registrations/dtos/approve-registration-file.dto";
import { SsoUserDto } from "src/auth/dtos/sign-up/sso-user.dto";
import { SettlementChangeService } from "src/shared/services/settlement-change/settlement-change.service";

@Component({
    selector: 'user-sign-up',
    templateUrl: './user-sign-up.component.html',
    providers: [SettlementChangeService],
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class UserSignUpComponent {

    @Input() isEditMode = true;
    @Input() disableUsername = false;
    @Input() file: ApproveRegistrationFileDto;
    @Input() showFile = false;
    @Input() showSampleFile = false;

    @Output() updateFile: EventEmitter<ApproveRegistrationFileDto> = new EventEmitter<ApproveRegistrationFileDto>();

    userDto: SsoUserDto;
    @Input('userDto')
    set userDtoSetter(userDto: SsoUserDto) {
        this.userDto = userDto;
    }

    fileChanged(file: ApproveRegistrationFileDto) {
        this.updateFile.emit(file);
    }

    constructor(
        public settlementChangeService: SettlementChangeService,
        public translateService: TranslateService) {

    }
}