import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { CompanyRoutingModule } from "./company.routing";
import { CompanyResource } from "./resources/company.resource";
import { CompanySearchComponent } from "./components/search/company-search.component";
import { CompanyDetailsComponent } from "./components/company-details.component";
import { CompanyAdditionalResource } from "./resources/company-additional.resource";
import { CompanyAdditionalComponent } from "./components/details/company-additional.component";
import { CompanyAdditionalBasicComponent } from "./components/details/company-additionals/basic/company-additional-basic.component";
import { CompanyAdditionalAddModalComponent } from "./components/details/company-additionals/modals/company-additional-add-modal.component";
import { CompanyAdditionalEditModalComponent } from "./components/details/company-additionals/modals/company-additional-edit-modal.component";
import { CompanySearchCardComponent } from "./components/search/company-search-card.component";

@NgModule({
    declarations: [
        CompanySearchComponent,
        CompanySearchCardComponent,
        CompanyDetailsComponent,
        CompanyAdditionalComponent,
        CompanyAdditionalBasicComponent,
        CompanyAdditionalAddModalComponent,
        CompanyAdditionalEditModalComponent
    ],
    imports: [
        CompanyRoutingModule,
        SharedModule
    ],
    providers: [
        CompanyResource,
        CompanyAdditionalResource
    ]
})
export class CompanyModule { }
