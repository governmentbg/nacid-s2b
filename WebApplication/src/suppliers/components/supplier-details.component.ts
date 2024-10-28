import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { SupplierDto } from '../dtos/supplier.dto';
import { equipmentDetailsTab, offeringDetailsTab, subordinateDetailsTab, teamDetailsTab } from '../constants/supplier-details.constants';
import { SupplierResource } from '../resources/supplier.resource';
import { SupplierType } from '../enums/supplier-type.enum';
import { InstitutionDto } from 'src/nomenclatures/dtos/institutions/institution.dto';
import { InstitutionResource } from 'src/nomenclatures/resources/institution.resource';
import { MessageModalComponent } from 'src/shared/components/message-modal/message-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'supplier-details',
    templateUrl: './supplier-details.component.html'
})
export class SupplierDetailsComponent implements OnInit {

    supplier = new SupplierDto();
    subordinates: InstitutionDto[] = [];
    loadingData = false;

    activeTab = teamDetailsTab;
    offeringDetailsTab = offeringDetailsTab;
    teamDetailsTab = teamDetailsTab;
    equipmentDetailsTab = equipmentDetailsTab;
    subordinateDetailsTab = subordinateDetailsTab;

    supplierType = SupplierType;

    constructor(
        private route: ActivatedRoute,
        private supplierResource: SupplierResource,
        private institutionResource: InstitutionResource,
        private modalService: NgbModal
    ) { }

    openTeamModalInfo(event: Event) {
        event.preventDefault();

        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.title = 'root.buttons.info'
        modal.componentInstance.text = 'supplierTeams.modals.infoModal';
        modal.componentInstance.infoOnly = true;
    }

    openEquipmentModalInfo(event: Event) {
        event.preventDefault();

        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.title = 'root.buttons.info'
        modal.componentInstance.text = 'supplierEquipment.modals.infoModal';
        modal.componentInstance.infoOnly = true;
    }

    private getSubordinates(parentId: number) {
        this.institutionResource.getSubordinates(parentId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.subordinates = e;
                this.loadingData = false;
            });
    }

    ngOnInit() {
        this.loadingData = true;
        this.route.params.subscribe(p => {
            this.supplierResource.getById(p['id'])
                .pipe(
                    catchError((err: HttpErrorResponse) => {
                        this.loadingData = false;
                        return throwError(() => err);
                    })
                )
                .subscribe(e => {
                    this.activeTab = (p['tab'] || teamDetailsTab);
                    this.supplier = e;
                    if (this.supplier.type === this.supplierType.institution) {
                        this.getSubordinates(this.supplier.institution.id);
                    } else {
                        this.loadingData = false;
                    }
                });
        });
    }
}
