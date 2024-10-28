import { Component, Input } from "@angular/core";

@Component({
    selector: 'dropdown-button',
    templateUrl: 'dropdown-button.component.html'
})
export class DropdownButtonComponent {

    @Input() icon: string;
    @Input() text: string;
    @Input() btnClass = 'btn-outline-primary btn-sm';
    @Input() fromEnd = false;
    @Input() loading = false;
    @Input() disabled = false;
}