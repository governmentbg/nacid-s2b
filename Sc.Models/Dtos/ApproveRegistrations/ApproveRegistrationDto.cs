namespace Sc.Models.Dtos.ApproveRegistrations
{
    public class ApproveRegistrationDto
    {
        public int RegistrationId { get; set; }
        public string RecaptchaToken { get; set; }
    }
}
