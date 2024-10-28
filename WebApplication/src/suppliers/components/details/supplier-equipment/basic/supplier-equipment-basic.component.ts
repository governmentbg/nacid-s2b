import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { Level } from "src/shared/enums/level.enum";
import { SupplierEquipmentDto } from "src/suppliers/dtos/supplier-equipment.dto";

@Component({
    selector: 'supplier-equipment-basic',
    templateUrl: './supplier-equipment-basic.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class SupplierEquipmentBasicComponent {

    @Input() supplierId: number;
    @Input() supplierEquipmentDto: SupplierEquipmentDto = new SupplierEquipmentDto();
    @Input() includeEquipmentOfferings = true;
    @Input() isEditMode = false;

    @Output() triggerAddEvent: EventEmitter<void> = new EventEmitter<void>();
    @Output() triggerEraseEvent: EventEmitter<number> = new EventEmitter<number>();

    excludedSupplierOfferingIds: number[] = [];

    level = Level;

    constructor(public translateService: TranslateService) {

    }

    eraseSoEquipment(index: number) {
        this.triggerEraseEvent.emit(index);
        this.constructExcludedIds();
    }

    soEquipmentChanged(id: number, index: number) {
        this.supplierEquipmentDto.supplierOfferingEquipment[index].supplierOfferingId = id;
        this.constructExcludedIds();
    }

    private constructExcludedIds() {
        this.excludedSupplierOfferingIds = this.supplierEquipmentDto.supplierOfferingEquipment
            .filter(e => e.supplierOfferingId)
            .map(e => e.supplierOfferingId);
    }

    ngOnInit() {
        this.constructExcludedIds();
    }
}