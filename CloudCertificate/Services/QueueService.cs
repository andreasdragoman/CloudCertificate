using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace CloudCertificate.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _config;

        public QueueService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            var client = new ServiceBusClient(_config.GetConnectionString("AzureServiceBus"), clientOptions);
            var sender = client.CreateSender(queueName);

            try
            {
                string messageBody = JsonSerializer.Serialize(serviceBusMessage);
                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
                await sender.SendMessageAsync(message);
            }
            finally {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
