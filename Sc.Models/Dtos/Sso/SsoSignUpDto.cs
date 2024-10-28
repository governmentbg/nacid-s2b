namespace Sc.Models.Dtos.Sso
{
    public class SsoSignUpDto
    {
        public string RecaptchaToken { get; set; }

        public string ClientId { get; set; }
        public string ActivationLinkUrl { get; set; }
        public string SystemText { get; set; }

        public string RoleAlias { get; set; }
        public bool DeleteUnitUserWithSameRole { get; set; } = false;
        public SsoUserDto User { get; set; }
        public SsoOrganizationalUnitDto OrganizationalUnit { get; set; }

        // For registration. If is company user must not duplicate unit user in SSO 
        public bool ThrowErrorIfExists { get; set; }
    }
}
