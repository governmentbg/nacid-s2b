using Sc.Models.Dtos.ApproveRegistrations;

namespace Sc.Models.Dtos.Auth
{
    public class SignUpDeclarationDto
    {
        public SignUpDto SignUp { get; set; }
        public ApproveRegistrationFileDto File { get; set; }
    }
}
