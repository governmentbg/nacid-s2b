import { SupplierOfferingTeamDto } from "./junctions/supplier-offering-team.dto";
import { SupplierDto } from "./supplier.dto";

export class SupplierTeamDto {
    id: number;

    supplierId: number;
    supplier: SupplierDto;

    userId: number;
    userName: string;
    userNameAgain: string;

    position: string;
    academicRankDegree: string;

    firstName: string;
    middleName: string;
    lastName: string;
    name: string;

    phoneNumber: string;

    rasLotId: number;
    rasLotNumber: number;
    rasPortalUrl: string;

    isActive: boolean;

    supplierOfferingTeams: SupplierOfferingTeamDto[] = [];

    // Client only

    rasBasic: any;
}