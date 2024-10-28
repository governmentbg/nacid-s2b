import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { OfferingType } from "src/suppliers/enums/offering-type.enum";
import { Level } from "src/shared/enums/level.enum";
import { SettlementChangeService } from "src/shared/services/settlement-change/settlement-change.service";
import { SupplierOfferingDto } from "src/suppliers/dtos/supplier-offering.dto";
import { SupplierEquipmentAddModalComponent } from "../../supplier-equipment/modals/supplier-equipment-add-modal.component";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SupplierEquipmentDto } from "src/suppliers/dtos/supplier-equipment.dto";
import { ScAttachedFileDto } from "src/shared/dtos/sc-attached-file.dto";
import { SupplierOfferingEquipmentDto } from "src/suppliers/dtos/junctions/supplier-offering-equipment.dto";

@Component({
    selector: 'supplier-offering-basic',
    templateUrl: './supplier-offering-basic.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class SupplierOfferingBasicComponent implements OnInit {

    level = Level;
    offeringType = OfferingType;
    excludedSmartSpecializationIds: number[] = [];
    excludedSupplierTeamIds: number[] = [];
    excludedSupplierEquipmentIds: number[] = [];
    excludedSupplierFileIds: number[] = [];
    supplierEquipment: SupplierEquipmentDto[] = [];

    @Input() supplierId: number;
    @Input() supplierOfferingDto: SupplierOfferingDto = new SupplierOfferingDto();
    @Input() isEditMode = false;

    @Output() triggerAddEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() triggerEraseEvent: EventEmitter<number> = new EventEmitter<number>();
    @Output() triggerTeamAddEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() triggerTeamEraseEvent: EventEmitter<number> = new EventEmitter<number>();
    @Output() triggerEquipmentAddEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() triggerEquipmentEraseEvent: EventEmitter<number> = new EventEmitter<number>();
    @Output() triggerFileAddEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() triggerFileEraseEvent: EventEmitter<number> = new EventEmitter<number>();
    constructor(
        public translateService: TranslateService,
        public settlementChangeService: SettlementChangeService,
        public modalService: NgbModal) {
    }

    eraseSmartSpecialization(index: number) {
        this.triggerEraseEvent.emit(index);
        this.constructExcludedIds();
    }

    eraseSoTeam(index: number) {
        this.triggerTeamEraseEvent.emit(index);
        this.constructTeamExcludedIds();
    }

    eraseSoEquipment(index: number) {
        this.triggerEquipmentEraseEvent.emit(index);
        this.constructEquipmentExcludedIds();
    }

    eraseSoFile(index: number) {
        this.triggerFileEraseEvent.emit(index);
    }

    smartSpecializationChanged(id: number, index: number) {
        this.supplierOfferingDto.smartSpecializations[index].smartSpecializationId = id;
        this.constructExcludedIds();
    }

    supplierTeamChanged(id: number, index: number) {
        this.supplierOfferingDto.supplierOfferingTeams[index].supplierTeamId = id;
        this.constructTeamExcludedIds();
    }

    supplierEquipmentChanged(id: number, index: number) {
        this.supplierOfferingDto.supplierOfferingEquipment[index].supplierEquipmentId = id;
        this.constructEquipmentExcludedIds();
    }

    disableSoSmartSpecializationAdd() {
        return this.supplierOfferingDto.smartSpecializations.some(e => !e.smartSpecializationId);
    }

    disableSoTeamAdd() {
        return this.supplierOfferingDto.supplierOfferingTeams.some(e => !e.supplierTeamId);
    }

    disableSoEquipmentAdd() {
        return this.supplierOfferingDto.supplierOfferingEquipment.some(e => !e.supplierEquipmentId);
    }

    disableSoFileAdd() {
        return this.supplierOfferingDto.files.length > 2 || this.supplierOfferingDto.files.some(e => !e?.name)
    }

    openAddSupplierEquipment() {
        const modal = this.modalService.open(SupplierEquipmentAddModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierId = this.supplierId;
        modal.componentInstance.includeEquipmentOfferings = false;

        return modal.result.then((newSupplierEquipmentDto: SupplierEquipmentDto) => {
            if (newSupplierEquipmentDto) {
                const newOfferingEquipment = new SupplierOfferingEquipmentDto()
                newOfferingEquipment.supplierEquipment = newSupplierEquipmentDto;
                newOfferingEquipment.supplierEquipmentId = newSupplierEquipmentDto.id;

                this.supplierOfferingDto.supplierOfferingEquipment.push(newOfferingEquipment);
            }
        });
    }

    private constructExcludedIds() {
        this.excludedSmartSpecializationIds = this.supplierOfferingDto.smartSpecializations
            .filter(e => e.smartSpecializationId)
            .map(e => e.smartSpecializationId);
    }

    private constructTeamExcludedIds() {
        this.excludedSupplierTeamIds = this.supplierOfferingDto.supplierOfferingTeams
            .filter(e => e.supplierTeamId)
            .map(e => e.supplierTeamId);
    }

    private constructEquipmentExcludedIds() {
        this.excludedSupplierEquipmentIds = this.supplierOfferingDto.supplierOfferingEquipment
            .filter(e => e.supplierEquipmentId)
            .map(e => e.supplierEquipmentId);
    }

    ngOnInit() {
        this.constructExcludedIds();
        this.constructTeamExcludedIds();
        this.constructEquipmentExcludedIds();
    }
}