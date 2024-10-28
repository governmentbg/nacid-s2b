import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'questionaire',
    templateUrl: './questionaire.component.html',
    styleUrls: ['./questionaire.component.styles.css']
})
export class QuestionaireComponent {

    constructor(public translateService: TranslateService) {
    }
}
