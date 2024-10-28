import { CompanyDto } from "src/companies/dtos/company.dto";
import { SsoUserDto } from "./sso-user.dto";
import { SignUpType } from "src/auth/enums/sign-up-type.enum";
import { InstitutionDto } from "src/nomenclatures/dtos/institutions/institution.dto";
import { SsoUserValidateSignUpInfoDto } from "./sso-user-validate-sign-up-info.dto";
import { SupplierType } from "src/suppliers/enums/supplier-type.enum";
import { ComplexDto } from "src/nomenclatures/dtos/complexes/complex.dto";
import { SupplierExtendedType } from "src/auth/enums/supplier-extended-type.enum";

export class SignUpDto {
    recaptchaToken: string;

    type = SignUpType.supplier;
    supplierType = SupplierType.institution;
    supplierExtendedType = SupplierExtendedType.university;

    user: SsoUserDto = new SsoUserDto();

    institution: InstitutionDto = null;
    complex: ComplexDto = null;

    company: CompanyDto = new CompanyDto();

    // Client only
    rootInstitution: InstitutionDto = null;

    // Info only
    ssoUserValidateSignUpInfo: SsoUserValidateSignUpInfoDto = new SsoUserValidateSignUpInfoDto();
}