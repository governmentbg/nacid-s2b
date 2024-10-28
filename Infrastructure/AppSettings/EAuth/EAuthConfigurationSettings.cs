namespace Infrastructure.AppSettings.EAuth
{
    public class EAuthConfigurationSettings
    {
        public string Url { get; set; }
        public string ProviderName { get; set; }
        public string ProviderId { get; set; }
        public string RequestUrl { get; set; }
        public string CertificateId { get; set; }
        public string ExtProviderId { get; set; }
        public string SecurityLevel { get; set; }
        public string RequestSignCertificate { get; set; }
        public string RequestSignCertificatePass { get; set; }
        public string ResponseSignCertificateThumbprint { get; set; }
        public string ResponseSignCertificateValidateExpirationDate { get; set; }
    }
}
