import { HttpErrorResponse } from "@angular/common/http";
import { Component, Input, OnInit } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { catchError, throwError } from "rxjs";
import { PermissionService } from "src/auth/services/permission.service";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { IsActiveDto } from "src/shared/dtos/is-active.dto";
import { complexAlias, nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { supplierTeamCreatePermission, supplierTeamDeletePermission, supplierTeamWritePermission } from "src/auth/constants/permission.constants";
import { SupplierTeamDto } from "src/suppliers/dtos/supplier-team.dto";
import { SupplierDto } from "src/suppliers/dtos/supplier.dto";
import { SupplierTeamResource } from "src/suppliers/resources/supplier-team.resource";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { SupplierTeamEditModalComponent } from "./supplier-team/modals/supplier-team-edit-modal.component";
import { SupplierTeamAddModalComponent } from "./supplier-team/modals/supplier-team-add-modal.component";
import { UserContextService } from "src/auth/services/user-context.service";

@Component({
    selector: 'supplier-team',
    templateUrl: './supplier-team.component.html',
})
export class SupplierTeamComponent implements OnInit {

    hasSupplierTeamsCreatePermission = false;
    hasSupplierTeamsWritePermission = false;
    hasSupplierTeamsDeletePermission = false;
    isSupplier = this.userContextService.isSupplier();

    supplierTeam: SupplierTeamDto[] = [];

    supplierType = SupplierType;
    loadingData = false;
    deleteSupplierTeamPending: boolean[] = [];
    changeIsActiveSupplierTeamPending: boolean[] = [];

    @Input() supplier: SupplierDto;

    constructor(
        private resource: SupplierTeamResource,
        private permissionService: PermissionService,
        private modalService: NgbModal,
        public userContextService: UserContextService
    ) {
    }

    openAddSupplierTeam() {
        const modal = this.modalService.open(SupplierTeamAddModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierId = this.supplier.id;
        modal.componentInstance.supplierType = this.supplier.type;
        modal.componentInstance.supplierInstitutionId = this.supplier.institutionId;

        return modal.result.then((newSupplierTeamDto: SupplierTeamDto) => {
            if (newSupplierTeamDto) {
                this.supplierTeam.unshift(newSupplierTeamDto);
            }
        });
    }

    openEditSupplierTeam(personInTeam: SupplierTeamDto, index: number) {
        const modal = this.modalService.open(SupplierTeamEditModalComponent, { backdrop: 'static', size: 'xl', keyboard: false });
        modal.componentInstance.supplierTeamId = personInTeam.id;
        modal.componentInstance.supplierId = this.supplier.id;
        modal.componentInstance.supplierType = this.supplier.type;
        modal.componentInstance.supplierInstitutionId = this.supplier.institutionId;

        return modal.result.then((updatedSupplierTeamDto: SupplierTeamDto) => {
            if (updatedSupplierTeamDto) {
                this.supplierTeam[index] = updatedSupplierTeamDto;
            }
        });
    }

    changeIsActive(personInTeam: SupplierTeamDto, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.text = 'root.modals.changeState';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure';
        if (personInTeam.isActive) {
            modal.componentInstance.title = 'supplierTeams.modals.deactivateTitle';
        } else {
            modal.componentInstance.title = 'supplierTeams.modals.activateTitle';
        }

        return modal.result.then((ok: boolean) => {
            if (ok) {
                this.changeIsActiveSupplierTeamPending[index] = true;

                var isActiveDto = new IsActiveDto();
                isActiveDto.id = personInTeam.id;
                isActiveDto.isActive = !personInTeam.isActive;

                this.resource.changeIsActive(this.supplier.id, isActiveDto)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.changeIsActiveSupplierTeamPending[index] = false;
                            return throwError(() => err);
                        })
                    )
                    .subscribe((isActive: boolean) => {
                        this.supplierTeam[index].isActive = isActive;
                        this.changeIsActiveSupplierTeamPending[index] = false;
                    });
            }
        });
    }

    deleteSupplierTeam(personInTeamId: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.text = 'supplierTeams.modals.deleteTeam';
        modal.componentInstance.text2 = 'supplierTeams.modals.confirmDelete';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure';
        modal.componentInstance.title = 'supplierTeams.modals.deleteTitle';

        return modal.result.then((ok: boolean) => {
            if (ok) {
                this.deleteSupplierTeamPending[index] = true;
                this.resource.delete(this.supplier.id, personInTeamId)
                    .pipe(
                        catchError((err: HttpErrorResponse) => {
                            this.deleteSupplierTeamPending[index] = false;
                            return throwError(() => err);
                        })
                    )
                    .subscribe(() => {
                        this.supplierTeam.splice(index, 1);
                        this.deleteSupplierTeamPending[index] = false;
                    });
            }
        });
    }

    private getTeamBySupplier(supplierId: number) {
        this.resource
            .getBySupplier(supplierId)
            .pipe(
                catchError((err: HttpErrorResponse) => {
                    this.loadingData = false;
                    return throwError(() => err);
                })
            )
            .subscribe(e => {
                this.supplierTeam = e;
                this.loadingData = false;
            });
    }

    ngOnInit() {
        this.loadingData = true;
        this.hasSupplierTeamsCreatePermission = (this.supplier.type === this.supplierType.institution
            ? this.permissionService.verifyUnitPermission(supplierTeamCreatePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
            : this.permissionService.verifyUnitPermission(supplierTeamCreatePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));
        this.hasSupplierTeamsWritePermission = (this.supplier.type === this.supplierType.institution
            ? this.permissionService.verifyUnitPermission(supplierTeamWritePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
            : this.permissionService.verifyUnitPermission(supplierTeamWritePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));
        this.hasSupplierTeamsDeletePermission = (this.supplier.type === this.supplierType.institution
            ? this.permissionService.verifyUnitPermission(supplierTeamDeletePermission, [[null, this.supplier.institutionId], [nacidAlias, null]])
            : this.permissionService.verifyUnitPermission(supplierTeamDeletePermission, [[complexAlias, this.supplier.complexId], [nacidAlias, null]]));

        this.getTeamBySupplier(this.supplier.id);
    }
}