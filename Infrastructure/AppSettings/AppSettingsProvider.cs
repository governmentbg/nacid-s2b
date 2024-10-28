using Infrastructure.AppSettings.EAuth;
using Infrastructure.AppSettings.Email;
using Infrastructure.AppSettings.MessageBroker;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.AppSettings
{
    public static class AppSettingsProvider
    {
        public static string MainDbConnectionString { get; private set; }
        public static string LogDbConnectionString { get; private set; }
        public static bool UseAllFunctionalities {  get; private set; }
        public static string Hosting { get; set; }
        public static SsoConfigurationSettings SsoConfiguration { get; private set; }
        public static RasIntegrationSettings RasIntegration { get; private set; }
        public static AgencyRegixIntegrationSettings AgencyRegixIntegration { get; private set; }
        public static SolrConfigurationSettings SolrConfiguration { get; private set; }
        public static EmailConfigurationSettings EmailConfiguration { get; private set; }
        public static MessageBrokerSettings MessageBroker { get; set; }
        public static EAuthConfigurationSettings EAuthConfiguration { get; private set; }
        public static string CertificateTemplatePath { get; set; }

        public static void AddAppSettings(IConfiguration configuration)
        {
            if (configuration.GetSection("mainDbConnectionString").Exists())
            {
                MainDbConnectionString = configuration.GetSection("mainDbConnectionString").Get<string>();
            }

            if (configuration.GetSection("logDbConnectionString").Exists())
            {
                LogDbConnectionString = configuration.GetSection("logDbConnectionString").Get<string>();
            }

            if (configuration.GetSection("useAllFunctionalities").Exists())
            {
                UseAllFunctionalities = configuration.GetSection("useAllFunctionalities").Get<bool>();
            }

            if (configuration.GetSection("hosting").Exists())
            {
                Hosting = configuration.GetSection("hosting").Get<string>();
            }

            if (configuration.GetSection("ssoConfiguration").Exists())
            {
                SsoConfiguration = configuration.GetSection("ssoConfiguration").Get<SsoConfigurationSettings>();
            }

            if (configuration.GetSection("rasIntegration").Exists())
            {
                RasIntegration = configuration.GetSection("rasIntegration").Get<RasIntegrationSettings>();
            }

            if (configuration.GetSection("agencyRegixIntegration").Exists())
            {
                AgencyRegixIntegration = configuration.GetSection("agencyRegixIntegration").Get<AgencyRegixIntegrationSettings>();
            }

            if (configuration.GetSection("solrConfiguration").Exists())
            {
                SolrConfiguration = configuration.GetSection("solrConfiguration").Get<SolrConfigurationSettings>();
            }

            if (configuration.GetSection("emailConfiguration").Exists())
            {
                EmailConfiguration = configuration.GetSection("emailConfiguration").Get<EmailConfigurationSettings>();
            }

            if (configuration.GetSection("messageBroker").Exists())
            {
                MessageBroker = configuration.GetSection("messageBroker").Get<MessageBrokerSettings>();
            }

            if (configuration.GetSection("eAuthConfiguration").Exists())
            {
                EAuthConfiguration = configuration.GetSection("eAuthConfiguration").Get<EAuthConfigurationSettings>();
            }

            if (configuration.GetSection("certificateTemplatePath").Exists())
            {
                CertificateTemplatePath = configuration.GetSection("certificateTemplatePath").Get<string>();
            }
        }
    }
}
