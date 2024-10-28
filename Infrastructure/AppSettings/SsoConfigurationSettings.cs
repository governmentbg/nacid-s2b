namespace Infrastructure.AppSettings
{
    public class SsoConfigurationSettings
    {
        public string SsoUri { get; set; }
        public string PasswordGrandType { get; set; }
        public string CodeGrandType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
