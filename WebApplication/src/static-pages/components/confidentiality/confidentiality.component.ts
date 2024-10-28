import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'confidentiality',
    templateUrl: './confidentiality.component.html',
    styleUrls: ['./confidentiality.component.styles.css']
})
export class ConfidentialityComponent {

    constructor(public translateService: TranslateService) {
    }

    purposes: string[] = ['confidentiality.purposesOfProcessing.firstPoint', 
        'confidentiality.purposesOfProcessing.secondPoint', 'confidentiality.purposesOfProcessing.thirdPoint',
        'confidentiality.purposesOfProcessing.fourthPoint']
}
