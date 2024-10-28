import { SettlementDto } from "src/nomenclatures/dtos/settlements/settlement.dto";
import { OfferingType } from "../enums/offering-type.enum";
import { MunicipalityDto } from "src/nomenclatures/dtos/settlements/municipality.dto";
import { DistrictDto } from "src/nomenclatures/dtos/settlements/district.dto";
import { SupplierOfferingFileDto } from "./supplier-offering-file.dto";
import { SupplierOfferingSmartSpecializationDto } from "./junctions/supplier-offering-smart-specialization.dto";
import { SupplierOfferingTeamDto } from "./junctions/supplier-offering-team.dto";
import { SupplierOfferingEquipmentDto } from "./junctions/supplier-offering-equipment.dto";
import { SupplierDto } from "./supplier.dto";

export class SupplierOfferingDto {
    id: number;

    supplierId: number;
    supplier: SupplierDto;

    code: string;

    offeringType: OfferingType

    name: string;
    shortDescription: string;
    description: string;

    settlementId: number;
    settlement: SettlementDto;
    municipalityId: number;
    municipality: MunicipalityDto;
    districtId: number;
    district: DistrictDto;
    address: string;
    webPageUrl: string;

    isActive: boolean;

    files: SupplierOfferingFileDto[] = [];

    smartSpecializations: SupplierOfferingSmartSpecializationDto[] = [];

    supplierOfferingTeams: SupplierOfferingTeamDto[] = [];
    supplierOfferingEquipment: SupplierOfferingEquipmentDto[] = [];
}