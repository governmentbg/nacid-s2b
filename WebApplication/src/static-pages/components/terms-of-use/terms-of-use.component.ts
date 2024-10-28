import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'terms-of-use',
    templateUrl: './terms-of-use.component.html',
    styleUrls: ['./terms-of-use.component.styles.css']
})
export class TermsOfUseComponent {

    constructor(public translateService: TranslateService) {
    }
}
