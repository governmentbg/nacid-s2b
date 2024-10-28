import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { HomeComponent } from "./components/home/home.component";
import { PageNotFoundComponent } from "./components/page-not-found/page-not-found.component";
import { StaticPageRoutingModule } from "./static-page.routing";
import { VoucherInfoComponent } from "./components/vouchers/voucher-info.component";
import { SupplierModule } from "src/suppliers/supplier.module";
import { CookiesPolicyComponent } from "./components/cookies-policy/cookies-policy.component";
import { VoucherTabsComponent } from "./components/vouchers/voucher-tabs.component";
import { QuestionaireComponent } from "./components/vouchers/questionaire.component";
import { TermsOfUseComponent } from "./components/terms-of-use/terms-of-use.component";
import { ConfidentialityComponent } from "./components/confidentiality/confidentiality.component";

@NgModule({
  declarations: [
    HomeComponent,
    VoucherInfoComponent,
    PageNotFoundComponent,
    CookiesPolicyComponent,
    VoucherTabsComponent,
    QuestionaireComponent,
    TermsOfUseComponent,
    ConfidentialityComponent
  ],
  imports: [
    StaticPageRoutingModule,
    SupplierModule,
    SharedModule
  ],
  providers: [
  ]
})
export class StaticPageModule { }
