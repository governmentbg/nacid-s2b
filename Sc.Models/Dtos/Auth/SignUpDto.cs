using Sc.Models.Dtos.Companies;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Dtos.Sso;
using Sc.Models.Enums.Auth;
using Sc.Models.Enums.Suppliers;

namespace Sc.Models.Dtos.Auth
{
    public class SignUpDto
    {
        public string RecaptchaToken { get; set; }

        public SignUpType Type { get; set; }
        public SupplierType SupplierType { get; set; }
        public SupplierExtendedType SupplierExtendedType { get; set; }

        public SsoUserDto User { get; set; }

        // Type = Supplier and SupplierType = Institution
        public InstitutionDto RootInstitution { get; set; }
        public InstitutionDto Institution { get; set; }
        // Type = Supplier and SupplierType = Complex
        public ComplexDto Complex { get; set; }

        // Type = Company
        public CompanyDto Company { get; set; }

        // Info only
        public SsoUserValidateSignUpInfoDto SsoUserValidateSignUpInfo { get; set; }
    }
}
