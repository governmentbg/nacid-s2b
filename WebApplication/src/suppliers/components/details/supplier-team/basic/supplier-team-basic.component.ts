import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { SupplierTeamDto } from "src/suppliers/dtos/supplier-team.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";

@Component({
    selector: 'supplier-team-basic',
    templateUrl: './supplier-team-basic.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class SupplierTeamBasicComponent {

    @Input() supplierId: number;
    @Input() supplierTeamDto: SupplierTeamDto = new SupplierTeamDto();
    @Input() supplierType: SupplierType;
    @Input() supplierInstitutionId: number;
    @Input() isEditMode = false;
    @Input() updateModal: boolean;

    @Output() triggerAddEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() triggerEraseEvent: EventEmitter<number> = new EventEmitter<number>();

    excludedSupplierOfferingIds: number[] = [];

    supplierTypeEnum = SupplierType;

    eraseSoTeam(index: number) {
        this.triggerEraseEvent.emit(index);
        this.constructExcludedIds();
    }

    soTeamChanged(id: number, index: number) {
        this.supplierTeamDto.supplierOfferingTeams[index].supplierOfferingId = id;
        this.constructExcludedIds();
    }

    rasBasicChanged(rasBasic: any) {
        this.supplierTeamDto.rasLotId = rasBasic?.lotId;
        this.supplierTeamDto.rasLotNumber = rasBasic?.lotNumber;
    }

    private constructExcludedIds() {
        this.excludedSupplierOfferingIds = this.supplierTeamDto.supplierOfferingTeams
            .filter(e => e.supplierOfferingId)
            .map(e => e.supplierOfferingId);
    }

    ngOnInit() {
        this.constructExcludedIds();
    }
}