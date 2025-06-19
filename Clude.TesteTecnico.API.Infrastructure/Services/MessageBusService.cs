using Azure.Messaging.ServiceBus;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Clude.TesteTecnico.API.Infrastructure.Services
{
    public class MessageBusService : IMessageBusService, IAsyncDisposable
    {
        private readonly ServiceBusClient _client;
        private readonly string _queueName;

        public MessageBusService(IOptions<ServiceBusSettings> settings)
        {
            var serviceBusSettings = settings.Value;
            _client = new ServiceBusClient(serviceBusSettings.ConnectionString);
            _queueName = serviceBusSettings.QueueName;
        }

        public async Task EnviarMensagemAsync(object mensagem)
        {
            var sender = _client.CreateSender(_queueName);

            string body = JsonSerializer.Serialize(mensagem);
            var message = new ServiceBusMessage(body);

            await sender.SendMessageAsync(message);
        }

        public async ValueTask DisposeAsync()
        {
            if (_client != null)
            {
                await _client.DisposeAsync();
            }
        }
    }
}
