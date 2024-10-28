using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace MessageBroker.Consumer.Jobs.Base
{
    public class BaseConsumerJob : BackgroundService
    {
        private IModel channel;
        private string queueName;
        private readonly ScMbConsumer scMbConsumer;

        public BaseConsumerJob(
            string exchangeName,
            ScMbConsumer scMbConsumer
        )
        {
            this.scMbConsumer = scMbConsumer;
            InitializeChannel(exchangeName);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, bodyStream) => {
                var body = bodyStream.Body.ToArray();
                try
                {
                    await HandleBody(body);
                    channel.BasicAck(bodyStream.DeliveryTag, false);
                }
                catch (Exception)
                {
                    channel.BasicReject(bodyStream.DeliveryTag, true);
                }
            };

            channel.BasicConsume(queue: queueName,
                                autoAck: false,
                                consumer: consumer);

            return Task.CompletedTask;
        }

        protected virtual async Task HandleBody(byte[] body)
        {
            await Task.CompletedTask;
        }

        private void InitializeChannel(string exchangeName)
        {
            channel = scMbConsumer.connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                             exchange: exchangeName,
                             routingKey: "");
        }
    }
}
