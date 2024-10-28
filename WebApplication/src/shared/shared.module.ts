import { CommonModule, DatePipe } from "@angular/common";
import { LOCALE_ID, NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NgbActiveModal, NgbModule, NgbScrollSpyModule } from "@ng-bootstrap/ng-bootstrap";
import { RecaptchaFormsModule, RecaptchaModule } from "ng-recaptcha";
import { TranslateModule } from "@ngx-translate/core";
import { AlertContainerComponent } from "./components/alert-message/alert-container.component";
import { AlertMessageService } from "./components/alert-message/services/alert-message.service";
import { LoadingPageComponent } from "./components/loading-page/loading-page.component";
import { LoadingSectionComponent } from "./components/loading-section/loading-section.component";
import { MessageModalComponent } from "./components/message-modal/message-modal.component";
import { SyncButtonComponent } from "./components/sync-button/sync-button.component";
import { NoWhitespacesDirective } from "./directives/validation/no-whitespaces.directive";
import { PasswordDirective } from "./directives/validation/password.directive";
import { BreadcrumbComponent } from "./components/breadcrumb/breadcrumb.component";
import { RouterModule } from "@angular/router";
import { PageHandlingService } from "./services/page-handling/page-handling.service";
import { TranslateFieldComponent } from "./components/translate-field/translate-field.component";
import { SearchResultComponent } from "./components/search-result/search-result.component";
import { FormatNumberPipe } from "./pipes/format-number.pipe";
import { TranslateEnumComponent } from "./components/translate-enum/translate-enum.component";
import { NomenclaturePipe } from "./pipes/nomenclature.pipe";
import { NomenclatureSelectComponent } from "./components/nomenclature-select/nomenclature-select.component";
import { CollapsableLabelComponent } from "./components/collapsable-label/collapsable-label.component";
import { AutofocusDirective } from "./directives/autofocus.directive";
import { EnumSelectComponent } from "./components/enum-select/enum-select.component";
import { BoolSelectComponent } from "./components/bool-select/bool-select.component";
import { EmailDirective } from "./directives/validation/email.directive";
import { PhoneDirective } from "./directives/validation/phone.directive";
import { CustomRegexDirective } from "./directives/validation/custom-regex.directive";
import { StaticFileDownloadComponent } from "./components/static-file-download/static-file-download.component";
import { ConfirmDirective } from "./directives/validation/confirm.directive";
import { UicDirective } from "./directives/validation/uic.directive";
import { ClickStopPropagation } from "./directives/click-stop-propagation.directive";
import { CurrencyInputComponent } from "./components/currency-input/currency-input.component";
import { FileUploadResource } from "./components/file-upload/file-upload.resource";
import { FileUploadComponent } from "./components/file-upload/file-upload.component";
import { FileReadComponent } from "./components/file-read/file-read.component";
import { FilterSelectComponent } from "./components/filter-select/filter-select.component";
import { registerLocaleData } from '@angular/common';
import localeBg from '@angular/common/locales/bg';
import { DatetimeComponent } from "./components/date-time/date-time.component";
import { SearchResultCountComponent } from "./components/search-result-count/search-result-count.component";
import { SettlementChangeService } from "./services/settlement-change/settlement-change.service";
import { BlockCopyPasteDirective } from "./directives/block-copy-paste.directive";
import { CollapsableSectionComponent } from "./components/collapsable-section/collapsable-section.component";
import { RemainingSymbolsComponent } from "./components/remaining-symbols/remaining-symbols.component";
import { FileReadCustomTextComponent } from "./components/file-read-custom-text/file-read-custom-text.component";
import { FilterSummaryComponent } from "./components/filter-summary/filter-summary.component";
import { AgencyRegixResource } from "./regix/agency-regix.resource";
import { HighchartsChartModule } from 'highcharts-angular';
import { NotificationResource } from "src/signalR/notification/resources/notification.resource";
import { CombinedPasswordDirective } from "./directives/validation/combined-validation.directive";
import { DropdownButtonComponent } from "./components/dropdown-button/dropdown-button.component";

registerLocaleData(localeBg);

const components = [
    AlertContainerComponent,
    BreadcrumbComponent,
    BoolSelectComponent,
    CollapsableLabelComponent,
    CollapsableSectionComponent,
    CurrencyInputComponent,
    DatetimeComponent,
    DropdownButtonComponent,
    EnumSelectComponent,
    FileReadComponent,
    FileReadCustomTextComponent,
    FileUploadComponent,
    FilterSelectComponent,
    FilterSummaryComponent,
    LoadingPageComponent,
    LoadingSectionComponent,
    MessageModalComponent,
    NomenclatureSelectComponent,
    TranslateEnumComponent,
    TranslateFieldComponent,
    RemainingSymbolsComponent,
    SearchResultComponent,
    SearchResultCountComponent,
    StaticFileDownloadComponent,
    SyncButtonComponent,
    AutofocusDirective,
    BlockCopyPasteDirective,
    ClickStopPropagation,
    ConfirmDirective,
    CustomRegexDirective,
    EmailDirective,
    NoWhitespacesDirective,
    PasswordDirective,
    PhoneDirective,
    UicDirective,
    FormatNumberPipe,
    NomenclaturePipe,
    CombinedPasswordDirective
];

const providers = [
    NgbActiveModal,
    DatePipe,
    AlertMessageService,
    FileUploadResource,
    PageHandlingService,
    SettlementChangeService,
    AgencyRegixResource,
    NotificationResource,
    { provide: LOCALE_ID, useValue: 'bg' }
]

const commonModules = [
    CommonModule,
    FormsModule,
    RouterModule,
    TranslateModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    NgbModule,
    NgbScrollSpyModule,
    HighchartsChartModule
]

@NgModule({
    declarations: components,
    imports: commonModules,
    providers: [providers],
    exports: [...commonModules, ...components]
})
export class SharedModule { }