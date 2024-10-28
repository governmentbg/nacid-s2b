import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { SupplierType } from "../enums/supplier-type.enum";
import { ComplexDto } from "src/nomenclatures/dtos/complexes/complex.dto";
import { SupplierRepresentativeDto } from "./supplier-representative.dto";

export class SupplierDto {
    id: number;

    type: SupplierType;

    institutionId: number;
    institution: InstitutionDto;

    complexId: number;
    complex: ComplexDto;

    representative: SupplierRepresentativeDto;
}