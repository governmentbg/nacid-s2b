import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { selectedLanguage } from 'src/shared/constants/language.constants';

@Component({
    selector: 'language-select',
    templateUrl: 'language-select.component.html',
    styleUrls: ['./language-select.styles.css']
})
export class LanguageSelectComponent {

    constructor(
        public translateService: TranslateService,
        private titleService: Title
    ) {
    }

    changeLanguage() {
        let language: string = null;

        if (this.translateService.currentLang === 'bg') {
            language = 'en';
        } else {
            language = 'bg';
        }

        localStorage.setItem(selectedLanguage, language);
        this.translateService.use(language);
        this.translateService.get('root.title')
            .subscribe(translatedTitle => this.titleService.setTitle(translatedTitle));
    }
}
