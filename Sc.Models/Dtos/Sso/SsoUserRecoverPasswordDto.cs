namespace Sc.Models.Dtos.Sso
{
    public class SsoUserRecoverPasswordDto
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordAgain { get; set; }
    }
}
