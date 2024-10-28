import { Component, Input } from "@angular/core";

@Component({
    selector: 'remaining-symbols',
    templateUrl: 'remaining-symbols.component.html'
})
export class RemainingSymbolsComponent {

    @Input() maxLength: number = 0;
    @Input() textLength: number = 0;
    @Input() justifyContentClass = 'justify-content-end';
}