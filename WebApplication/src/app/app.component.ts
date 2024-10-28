import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { UserAuthorizationState } from 'src/auth/enums/user-authorization-state.enum';
import { UserContextService } from 'src/auth/services/user-context.service';
import { selectedLanguage } from 'src/shared/constants/language.constants';
import { Configuration } from './configuration/configuration';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  authorizationState = UserAuthorizationState;

  constructor(
    private titleService: Title,
    public userContextService: UserContextService,
    public configuration: Configuration,
    private translateService: TranslateService
  ) {
  }

  private setDefaultLanguage() {
    let language = localStorage.getItem(selectedLanguage);

    if (!language) {
      language = this.configuration.defaultLanguage;
      localStorage.setItem(selectedLanguage, language);
    }

    this.translateService.setDefaultLang(language);
    this.translateService.use(language);
    this.translateService.get('root.title')
      .subscribe(translatedTitle => this.titleService.setTitle(translatedTitle));
  }

  ngOnInit() {
    this.setDefaultLanguage();
  }
}
