using Infrastructure.AppSettings;
using Infrastructure.AppSettings.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sc.Models;
using Sc.Models.Enums.Emails;
using Sc.Services.Emails;

namespace Sc.Jobs.Emails
{
    public class EmailJob : IHostedService, IDisposable
    {
        private readonly EmailConfigurationSettings emailConfiguration;
        private readonly IServiceProvider serviceProvider;
        private Timer timer;

        public EmailJob(
            IServiceProvider serviceProvider
            )
        {
            emailConfiguration = AppSettingsProvider.EmailConfiguration;
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(emailConfiguration.JobPeriod));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public async void DoWork(object state)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ScDbContext>();
            var senderService = scope.ServiceProvider.GetRequiredService<EmailSenderService>();

            var unsentEmails = await senderService.GetUnsentEmails(emailConfiguration.JobLimit);

            foreach (var email in unsentEmails)
            {
                email.Status = EmailStatus.Pending;
                email.StartSendingDate = DateTime.Now;
            }

            await dbContext.SaveChangesAsync();

            foreach (var email in unsentEmails)
            {
                bool isSent = await senderService.SendEmail(email);

                if (isSent)
                {
                    email.Status = EmailStatus.Delivered;
                    email.SentDate = DateTime.Now;
                }
                else
                {
                    email.Status = EmailStatus.Failed;
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
