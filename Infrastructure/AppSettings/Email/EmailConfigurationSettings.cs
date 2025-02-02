﻿namespace Infrastructure.AppSettings.Email
{
    public class EmailConfigurationSettings
    {
        public int JobPeriod { get; set; }
        public int JobLimit { get; set; }
        public bool JobEnabled { get; set; }

        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool SmtpUseSsl { get; set; }
        public bool SmtpShouldAuthenticate { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        public List<string> SupplierRegistrationInfoMailsTo { get; set; } = new List<string>();
    }
}
