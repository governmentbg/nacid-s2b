import { Component, Input, OnChanges } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'translate-enum',
    templateUrl: 'translate-enum.component.html'
})
export class TranslateEnumComponent implements OnChanges {

    text: string = null;

    @Input() model: any = null;
    @Input() enumName: string;
    @Input() enumType: any;
    @Input() class: string;

    constructor(public translateService: TranslateService) {
    }

    ngOnChanges() {
        if (this.model) {
            this.text = `enums.${this.enumName}.${this.enumType[this.model]}`;
        }
    }
}
