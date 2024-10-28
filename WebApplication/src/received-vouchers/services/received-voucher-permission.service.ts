import { Injectable } from "@angular/core";
import { PermissionService } from "src/auth/services/permission.service";
import { ReceivedVoucherDto } from "../dtos/received-voucher.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { receivedVoucherWritePermission } from "src/auth/constants/permission.constants";
import { complexAlias } from "src/auth/constants/organizational-unit.constants";
import { UserContextService } from "src/auth/services/user-context.service";

@Injectable()
export class ReceivedVoucherPermissionService {

    constructor(
        private permissionService: PermissionService,
        private userContextService: UserContextService
    ) {
    }

    canGenerateCertificate(receivedVoucherDto: ReceivedVoucherDto) {
        if ((receivedVoucherDto.supplier != null && receivedVoucherDto.supplier.type === SupplierType.institution && this.permissionService.verifyUnitPermission(receivedVoucherWritePermission, [[null, receivedVoucherDto.supplier.institutionId]]))
            || (receivedVoucherDto.supplier != null && receivedVoucherDto.supplier.type === SupplierType.complex && this.permissionService.verifyUnitPermission(receivedVoucherWritePermission, [[complexAlias, receivedVoucherDto.supplier.complexId]]))
            || (receivedVoucherDto.secondSupplier != null && receivedVoucherDto.secondSupplier.type === SupplierType.institution && this.permissionService.verifyUnitPermission(receivedVoucherWritePermission, [[null, receivedVoucherDto.secondSupplier.institutionId]]))
            || (receivedVoucherDto.secondSupplier != null && receivedVoucherDto.secondSupplier.type === SupplierType.complex && this.permissionService.verifyUnitPermission(receivedVoucherWritePermission, [[complexAlias, receivedVoucherDto.secondSupplier.complexId]]))) {
            const generatedCertificates = receivedVoucherDto.certificates;

            if (!generatedCertificates || generatedCertificates?.length < 1) {
                return true;
            } else if (receivedVoucherDto.secondOfferingId && receivedVoucherDto.secondOfferingId > 0 ? generatedCertificates?.length > 1 : generatedCertificates?.length > 0) {
                return false;
            } else if (generatedCertificates?.length === 1) {
                const userSupplierIds = this.userContextService.userContext.organizationalUnits.filter(e => e.supplierId !== null).map(e => e.supplierId);
                const nonGeneratedSupplierId = generatedCertificates[0].offeringId === receivedVoucherDto.offeringId
                    ? receivedVoucherDto.secondSupplierId
                    : receivedVoucherDto.supplierId;

                if (userSupplierIds.includes(nonGeneratedSupplierId)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}