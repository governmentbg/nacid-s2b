import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from 'src/shared/shared.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AuthModule } from 'src/auth/auth.module';
import { Configuration } from './configuration/configuration';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { HeaderComponent } from './root/header/header.component';
import { LanguageSelectComponent } from './root/language-select/language-select.component';
import { LogoutAuthGuard } from './auth-guard/logout.auth-guard';
import { RECAPTCHA_SETTINGS, RECAPTCHA_LANGUAGE, RecaptchaSettings } from 'ng-recaptcha';
import { ProfileSidebarComponent } from './root/profile-sidebar/profile-sidebar.component';
import { StaticPageModule } from 'src/static-pages/static-page.module';
import { FooterComponent } from './root/footer/footer.component';
import { NomenclatureModule } from 'src/nomenclatures/nomenclature.module';
import { PermissionAuthGuard } from './auth-guard/permission.auth-guard';
import { CompanyModule } from 'src/companies/company.module';
import { LoginAuthGuard } from './auth-guard/login-auth-guard';
import { LogModule } from 'src/logs/log.module';
import { ApproveRegistrationsModule } from 'src/approve-registrations/approve-registrations.module';
import { SupplierModule } from 'src/suppliers/supplier.module';
import { VoucherRequestModule } from 'src/voucher-requests/voucher-request.module';
import { NotificationHubService } from 'src/signalR/notification-hub.service';
import { NotificationContainerComponent } from 'src/signalR/notification/components/notification-container.component';
import { HighchartReportModule } from 'src/highchart-reports/highchart-report.module';
import { BgMapJson } from './bg-map-json/bg-map-json';
import { ReceivedVoucherModule } from 'src/received-vouchers/received-voucher.module';
import { ReportModule } from 'src/reports/report.module';
import { EAuthModule } from 'src/e-auth/e-auth.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    ProfileSidebarComponent,
    LanguageSelectComponent,
    NotificationContainerComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    AuthModule,
    CompanyModule,
    SupplierModule,
    VoucherRequestModule,
    ReceivedVoucherModule,
    ReportModule,
    NomenclatureModule,
    LogModule,
    ApproveRegistrationsModule,
    HighchartReportModule,
    EAuthModule,
    SharedModule,
    StaticPageModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: createHttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    Configuration,
    BgMapJson,
    LogoutAuthGuard,
    LoginAuthGuard,
    PermissionAuthGuard,
    NotificationHubService,
    {
      provide: APP_INITIALIZER,
      useFactory: configFactory,
      deps: [Configuration],
      multi: true
    },
    {
      provide: APP_INITIALIZER,
      useFactory: bgMapFactory,
      deps: [BgMapJson],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: "6LebOIEmAAAAAEYy7MAq2mVBg6exU4jHvUxIoUax"
      } as RecaptchaSettings
    },
    {
      provide: RECAPTCHA_LANGUAGE,
      useValue: "bg"
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function configFactory(config: Configuration) {
  return () => config.load();
}

export function bgMapFactory(bgMapJson: BgMapJson) {
  return () => bgMapJson.load();
}

export function createHttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
