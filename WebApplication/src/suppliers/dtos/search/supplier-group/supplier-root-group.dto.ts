import { BaseSupplierGroupDto } from "./base/base-supplier-group.dto";
import { SupplierSubordinateGroupDto } from "./supplier-subordinate-group.dto";

export class SupplierRootGroupDto extends BaseSupplierGroupDto {
    supplierSubordinateGroup: SupplierSubordinateGroupDto[] = [];
}