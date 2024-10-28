namespace Sc.Models.Dtos.Sso
{
    public class SsoUserActivationDto
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
    }
}
