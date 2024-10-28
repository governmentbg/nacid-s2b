namespace Sc.Models.Dtos.Sso
{
    public class SsoChangePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordAgain { get; set; }
    }
}
