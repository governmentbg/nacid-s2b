using DocumentFormat.OpenXml.Vml;
using Infrastructure.AppSettings;
using Logs.Enums;
using Logs.Services;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Sc.Models;
using Sc.Models.Entities.Emails;
using Sc.Models.Enums.Emails;

namespace Sc.Services.Emails
{
    public class EmailSenderService
    {
        private readonly ScDbContext context;
        private readonly ErrorLogService errorLogService;

        public EmailSenderService(
            ScDbContext context,
            ErrorLogService errorLogService
            )
        {
            this.context = context;
            this.errorLogService = errorLogService;
        }

        public async Task<List<Email>> GetUnsentEmails(int limit)
        {
            return await context.Emails
                .Where(e => e.Status == EmailStatus.Unsent)
                .OrderBy(e => e.Id)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<bool> SendEmail(Email email)
        {
            var mimeMessage = TryComposeMimeMessage(email.To, AppSettingsProvider.EmailConfiguration.FromName, AppSettingsProvider.EmailConfiguration.FromAddress, email.Subject, email.Body);

            if (mimeMessage == null)
            {
                return false;
            }

            using var smtpClient = new SmtpClient();

            try
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                smtpClient.Connect(AppSettingsProvider.EmailConfiguration.SmtpHost, AppSettingsProvider.EmailConfiguration.SmtpPort, AppSettingsProvider.EmailConfiguration.SmtpUseSsl);

                if (AppSettingsProvider.EmailConfiguration.SmtpShouldAuthenticate)
                {
                    smtpClient.Authenticate(AppSettingsProvider.EmailConfiguration.SmtpUsername, AppSettingsProvider.EmailConfiguration.SmtpPassword);
                }

                smtpClient.Send(mimeMessage);

                return true;
            }
            catch (Exception exception)
            {
                var customText = $"SupplierRegistration info email to {email.To} not send.";

                await errorLogService.LogError(exception, ErrorLogType.EmailSenderExceptionLog, null, null, null, customText);
                return false;
            }
            finally
            {
                smtpClient.Disconnect(true);
            }
        }

        private MimeMessage TryComposeMimeMessage(string toAddress, string fromName, string fromAddress, string subject, string body)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(fromName, fromAddress));
                mimeMessage.To.Add(new MailboxAddress(toAddress, toAddress));

                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };

                return mimeMessage;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
