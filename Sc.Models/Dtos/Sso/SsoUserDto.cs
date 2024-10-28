namespace Sc.Models.Dtos.Sso
{
    public class SsoUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public SsoUserInfoDto UserInfo { get; set; }
    }
}
