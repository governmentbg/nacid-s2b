import { Component, Input, OnChanges } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'translate-field',
    templateUrl: 'translate-field.component.html'
})
export class TranslateFieldComponent implements OnChanges {

    bgText: string = null;
    enText: string = null;

    @Input() model: any = null;
    @Input() showTitle = false;
    @Input() bgProperty = 'name';
    @Input() enProperty = 'nameAlt';
    @Input() hasBrackets = false;
    @Input() class: string;

    constructor(public translateService: TranslateService) {
    }

    ngOnChanges() {
        if (this.model) {
            this.bgText = this.hasBrackets ? `(${this.model[this.bgProperty]})` : this.model[this.bgProperty];
            this.enText = this.hasBrackets ? `(${this.model[this.enProperty]})` : this.model[this.enProperty];
        }
    }
}
