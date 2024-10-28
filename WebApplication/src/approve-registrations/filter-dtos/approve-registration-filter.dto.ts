import { FilterDto } from "src/shared/filter-dtos/filter.dto";
import { ApproveRegistrationState } from "../enums/approve-registration-state.enum";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { ComplexDto } from "src/nomenclatures/dtos/complexes/complex.dto";

export class ApproveRegistrationFilterDto extends FilterDto {
  authorizedRepresentativeUsername: string;
  authorizedRepresentativeFullname: string;
  institutionId: number;
  institution: InstitutionDto;
  complexId: number;
  complex: ComplexDto;
  administratedUserId: number;
  state: ApproveRegistrationState;
}
