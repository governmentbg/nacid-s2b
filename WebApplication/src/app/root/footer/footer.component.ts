import { Component } from '@angular/core';
import { PageHandlingService } from 'src/shared/services/page-handling/page-handling.service';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.styles.css']
})
export class FooterComponent {

    currentYear = new Date().getFullYear();

    constructor(
        public pageHandlingService: PageHandlingService
    ) {
    }
}
