import { SharedModule } from "src/shared/shared.module";
import { SupplierOfferingGroupResource } from "./resources/supplier-offering-group.resource";
import { SupplierRoutingModule } from "./supplier.routing";
import { NgModule } from "@angular/core";
import { SupplierSearchGroupResource } from "./resources/supplier-search-group.resource";
import { SupplierOfferingSearchComponent } from "./components/search/supplier-offering/supplier-offering-search.component";
import { SmartSpecializationGroupComponent } from "./components/search/supplier-offering/smart-specialization-group/smart-specialization-group.component";
import { SmartSpecializationCardComponent } from "./components/search/supplier-offering/smart-specialization-group/smart-specialization-card.component";
import { SupplierOfferingGroupComponent } from "./components/search/supplier-offering/supplier-offering-group/supplier-offering-group.component";
import { SupplierOfferingGroupCardComponent } from "./components/search/supplier-offering/supplier-offering-group/supplier-offering-group-card.component";
import { SupplierSearchGroupComponent } from "./components/search/supplier-search-group/supplier-search-group.component";
import { SupplierSearchGroupCardComponent } from "./components/search/supplier-search-group/supplier-search-group-card.component";
import { SupplierSubordinateCardComponent } from "./components/search/supplier-search-group/supplier-subordinate-group/supplier-subordinate-card.component";
import { SupplierSmartSpecializationCardComponent } from "./components/search/supplier-search-group/supplier-smart-specialization/supplier-smart-specialization-card.component";
import { SupplierResource } from "./resources/supplier.resource";
import { SupplierInstitutionDetailsComponent } from "./components/supplier-institution-details.component";
import { SupplierDetailsComponent } from "./components/supplier-details.component";
import { SupplierRepresentativeComponent } from "./components/details/supplier-representative.component";
import { SupplierComplexDetailsComponent } from "./components/supplier-complex-details.component";
import { SupplierInstitutionSubordinatesComponent } from "./components/details/supplier-institution-subordinates.component";
import { SupplierOfferingResource } from "./resources/supplier-offering.resource";
import { SupplierOfferingComponent } from "./components/details/supplier-offering.component";
import { SupplierOfferingTrComponent } from "./components/details/supplier-offering-tr.component";
import { SupplierOfferingPermissionService } from "./services/supplier-offering-permission.service";
import { SupplierOfferingReadComponent } from "./components/details/supplier-offering/supplier-offering-read.component";
import { SupplierOfferingBasicComponent } from "./components/details/supplier-offering/basic/supplier-offering-basic.component";
import { SupplierOfferingEditModalComponent } from "./components/details/supplier-offering/modals/supplier-offering-edit-modal.component";
import { SupplierOfferingDeleteModalComponent } from "./components/details/supplier-offering/modals/supplier-offering-delete-modal.component";
import { SupplierOfferingAddModalComponent } from "./components/details/supplier-offering/modals/supplier-offering-add-modal.component";
import { SupplierTeamResource } from "./resources/supplier-team.resource";
import { SupplierTeamBasicComponent } from "./components/details/supplier-team/basic/supplier-team-basic.component";
import { SupplierTeamComponent } from "./components/details/supplier-team.component";
import { SupplierTeamEditModalComponent } from "./components/details/supplier-team/modals/supplier-team-edit-modal.component";
import { SupplierTeamAddModalComponent } from "./components/details/supplier-team/modals/supplier-team-add-modal.component";
import { SupplierEquipmentResource } from "./resources/supplier-equipment.resource";
import { SupplierEquipmentComponent } from "./components/details/supplier-equipment.component";
import { SupplierEquipmentBasicComponent } from "./components/details/supplier-equipment/basic/supplier-equipment-basic.component";
import { SupplierEquipmentEditModalComponent } from "./components/details/supplier-equipment/modals/supplier-equipment-edit-modal.component";
import { SupplierEquipmentAddModalComponent } from "./components/details/supplier-equipment/modals/supplier-equipment-add-modal.component";
import { VoucherRequestModule } from "src/voucher-requests/voucher-request.module";

@NgModule({
    declarations: [
        SupplierDetailsComponent,
        SupplierInstitutionDetailsComponent,
        SupplierComplexDetailsComponent,
        SupplierRepresentativeComponent,
        SupplierOfferingComponent,
        SupplierOfferingTrComponent,
        SupplierOfferingReadComponent,
        SupplierOfferingBasicComponent,
        SupplierOfferingAddModalComponent,
        SupplierOfferingEditModalComponent,
        SupplierOfferingDeleteModalComponent,
        SupplierTeamComponent,
        SupplierTeamBasicComponent,
        SupplierTeamAddModalComponent,
        SupplierTeamEditModalComponent,
        SupplierEquipmentComponent,
        SupplierEquipmentBasicComponent,
        SupplierEquipmentAddModalComponent,
        SupplierEquipmentEditModalComponent,
        SupplierInstitutionSubordinatesComponent,
        SupplierOfferingSearchComponent,
        SmartSpecializationGroupComponent,
        SmartSpecializationCardComponent,
        SupplierOfferingGroupComponent,
        SupplierOfferingGroupCardComponent,
        SupplierSearchGroupComponent,
        SupplierSearchGroupCardComponent,
        SupplierSubordinateCardComponent,
        SupplierSmartSpecializationCardComponent
    ],
    imports: [
        SupplierRoutingModule,
        VoucherRequestModule,
        SharedModule
    ],
    providers: [
        SupplierResource,
        SupplierOfferingResource,
        SupplierTeamResource,
        SupplierEquipmentResource,
        SupplierOfferingGroupResource,
        SupplierSearchGroupResource,
        SupplierOfferingPermissionService
    ],
    exports: [
        SupplierOfferingSearchComponent
    ]
})
export class SupplierModule { }