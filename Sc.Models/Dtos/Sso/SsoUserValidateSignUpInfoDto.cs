namespace Sc.Models.Dtos.Sso
{
    public class SsoUserValidateSignUpInfoDto
    {
        public bool Exists { get; set; } = false;

        public List<SsoUserApplicationUnitInfoDto> SsoApplicationUnits { get; set; } = new List<SsoUserApplicationUnitInfoDto>();
    }

    public class SsoUserApplicationUnitInfoDto
    {
        public string SsoApplicationName { get; set; }
        public List<string> OrganizationalUnitNames { get; set; }
    }
}
