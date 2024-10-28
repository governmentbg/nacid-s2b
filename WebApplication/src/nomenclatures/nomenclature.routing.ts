import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { SettlementTabsComponent } from "./components/tabs/settlement-tabs.component";
import { OthersTabsComponent } from "./components/tabs/others-tabs.component";

const routes: Routes = [
  {
    path: 'nomenclatures/settlements',
    component: SettlementTabsComponent,
    data: {
      title: 'routes.nomenclatures.settlemets',
    }
  },
  {
    path: 'nomenclatures/others',
    component: OthersTabsComponent,
    data: {
      title: 'routes.nomenclatures.others',
    }
  }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NomenclatureRoutingModule { }