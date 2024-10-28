import { Injectable } from "@angular/core";
import { PermissionService } from "src/auth/services/permission.service";
import { UserContextService } from "src/auth/services/user-context.service";
import { complexAlias, nacidAlias } from "src/auth/constants/organizational-unit.constants";
import { SupplierDto } from "../dtos/supplier.dto";
import { SupplierType } from "../enums/supplier-type.enum";
import { SupplierOfferingDto } from "../dtos/supplier-offering.dto";

@Injectable()
export class SupplierOfferingPermissionService {

    constructor(
        private userContextService: UserContextService,
        private permissionService: PermissionService) {
    }

    verifyOfferingPermissionException(permission: string, supplier: SupplierDto, supplierOffering: SupplierOfferingDto) {

        if (supplier.type === SupplierType.institution
            && !this.permissionService.verifyUnitPermission(permission, [[null, supplier.institutionId], [nacidAlias, null]])) {
            return false;
        } else if (supplier.type === SupplierType.complex
            && !this.permissionService.verifyUnitPermission(permission, [[complexAlias, supplier.complexId], [nacidAlias, null]])) {
            return false;
        }

        const offeringTeamsIds = supplierOffering.supplierOfferingTeams?.map(e => e?.supplierTeam?.userId);

        if ((supplier?.representative?.userId && this.userContextService.userContext.userId === supplier?.representative?.userId)
            || this.permissionService.verifyUnitPermission(permission, [[nacidAlias, null]])
            || (offeringTeamsIds.length > 0 && offeringTeamsIds.indexOf(this.userContextService.userContext.userId) > -1)) {
            return true;
        } else {
            return false;
        }
    }
}