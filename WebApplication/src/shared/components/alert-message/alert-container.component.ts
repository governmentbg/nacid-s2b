import { Component } from '@angular/core';
import { AlertMessageService } from './services/alert-message.service';

@Component({
    selector: 'alert-container',
    templateUrl: 'alert-container.component.html',
    host: { class: 'toast-container position-fixed top-0 end-0 p-3 cursor-pointer', style: 'z-index: 9999' },
})
export class AlertContainerComponent {

    constructor(public alertMessageService: AlertMessageService) {
    }
}