import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'sync-button',
    templateUrl: 'sync-button.component.html'
})
export class SyncButtonComponent {
    @Input() text: string;
    @Input() icon: string;
    @Input() btnClass: string;
    @Input() title: string;
    @Input() ngTitle: string;
    @Input() showTextOnPending = true;
    @Input() disabled: boolean = false;
    @Input() pending = false;

    @Output() btnClickedEvent: EventEmitter<void> = new EventEmitter<void>();

    btnClicked() {
        if (!this.disabled && !this.pending) {
            this.btnClickedEvent.emit();
        }
    }
}
