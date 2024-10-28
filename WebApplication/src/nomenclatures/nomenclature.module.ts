import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { NomenclatureRoutingModule } from "./nomenclature.routing";
import { SettlementTabsComponent } from './components/tabs/settlement-tabs.component';
import { SettlementSearchComponent } from "./components/settlement/settlement-search.component";
import { DistrictSearchComponent } from "./components/settlement/district-search.component";
import { MunicipalitySearchComponent } from "./components/settlement/municipality-search.component";
import { OthersTabsComponent } from './components/tabs/others-tabs.component';
import { LawFormSearchComponent } from './components/others/law-form-search.component';
import { SmartSpecializationSearchComponent } from './components/others/smart-specialization-search.component';
import { InstitutionResource } from "src/nomenclatures/resources/institution.resource";

@NgModule({
    declarations: [
        SettlementTabsComponent,
        SettlementSearchComponent,
        DistrictSearchComponent,
        MunicipalitySearchComponent,
        OthersTabsComponent,
        LawFormSearchComponent,
        SmartSpecializationSearchComponent,
    ],
    imports: [
        NomenclatureRoutingModule,
        SharedModule
    ],
    providers: [
        InstitutionResource
    ]
})
export class NomenclatureModule { }