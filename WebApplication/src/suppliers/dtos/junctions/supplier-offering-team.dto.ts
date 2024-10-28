import { SupplierOfferingDto } from "../supplier-offering.dto";
import { SupplierTeamDto } from "../supplier-team.dto";

export class SupplierOfferingTeamDto {
    id: number;

    supplierOfferingId: number;
    supplierOffering: SupplierOfferingDto;

    supplierTeamId: number;
    supplierTeam: SupplierTeamDto;
}