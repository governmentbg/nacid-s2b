using Infrastructure.AppSettings;
using Logs.Enums;
using Logs.Services;
using MessageBroker.Consumer.Jobs.Base;
using MessageBroker.Consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sc.Models.Entities.Nomenclatures.Institutions;
using System.Text;

namespace MessageBroker.Consumer.Jobs
{
    public class RndOrganizationUpdateJob : BaseConsumerJob
    {
        private readonly IServiceProvider serviceProvider;

        public RndOrganizationUpdateJob(
            IServiceProvider serviceProvider,
            ScMbConsumer scMbService
        ) : base(AppSettingsProvider.MessageBroker.ScConsumer.RndOrganizationUpdateExchange, scMbService)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task HandleBody(byte[] body)
        {
            using var scope = serviceProvider.CreateScope();
            var rndOrganizationUpdateService = scope.ServiceProvider
                .GetRequiredService<RndOrganizationUpdateService>();
            var logService = scope.ServiceProvider
                .GetRequiredService<ErrorLogService>();

            Institution organizationForUpdate = null;

            try
            {
                organizationForUpdate = JsonConvert.DeserializeObject<Institution>(Encoding.UTF8.GetString(body));

                if (organizationForUpdate != null) {
                    await rndOrganizationUpdateService.UpdateOrganization(organizationForUpdate);
                }
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                { exception = exception.InnerException; }

                var customText = $"Организация с Id: {organizationForUpdate?.Id} и Наименование: {organizationForUpdate?.Name} не е въведена/редактирана през message broker-а";

                await logService.LogError(exception, ErrorLogType.MessageBrokerExceptionLog, null, null, customText);
            }
        }
    }
}
