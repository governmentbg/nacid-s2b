using Infrastructure.AppSettings;
using Logs.Enums;
using Logs.Services;
using MessageBroker.Consumer.Jobs.Base;
using MessageBroker.Consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sc.Models.Entities.Nomenclatures.Complexes;
using System.Text;

namespace MessageBroker.Consumer.Jobs
{
    public class RndComplexUpdateJob : BaseConsumerJob
    {
        private readonly IServiceProvider serviceProvider;

        public RndComplexUpdateJob(
            IServiceProvider serviceProvider,
            ScMbConsumer scMbService
        ) : base(AppSettingsProvider.MessageBroker.ScConsumer.RndComplexUpdateExchange, scMbService)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task HandleBody(byte[] body)
        {
            using var scope = serviceProvider.CreateScope();
            var rndComplexUpdateService = scope.ServiceProvider
                .GetRequiredService<RndComplexUpdateService>();
            var logService = scope.ServiceProvider
                .GetRequiredService<ErrorLogService>();

            Complex complexForUpdate = null;

            try
            {
                complexForUpdate = JsonConvert.DeserializeObject<Complex>(Encoding.UTF8.GetString(body));
                await rndComplexUpdateService.UpdateComplex(complexForUpdate);
            }
            catch (Exception exception)
            {
                while (exception.InnerException != null)
                { exception = exception.InnerException; }

                var customText = $"Инфраструктура с Id: {complexForUpdate?.Id} и Наименование: {complexForUpdate?.Name} не е въведена/редактирана през message broker-а";

                await logService.LogError(exception, ErrorLogType.MessageBrokerExceptionLog, null, null, customText);
            }
        }
    }
}
