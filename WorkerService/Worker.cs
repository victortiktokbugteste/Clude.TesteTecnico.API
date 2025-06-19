using Azure.Messaging.ServiceBus;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Clude.TesteTecnico.API.Domain.Interfaces;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private ServiceBusProcessor _processor;

        public Worker(IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _config = config;
            _scopeFactory = scopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = new ServiceBusClient(_config["ServiceBus:ConnectionString"]);
            _processor = client.CreateProcessor(_config["ServiceBus:QueueName"], new ServiceBusProcessorOptions());

            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(cancellationToken);
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            var json = args.Message.Body.ToString();
            var data = JsonSerializer.Deserialize<AgendamentoMessage>(json);

            using var scope = _scopeFactory.CreateScope();
            var agendamentoRepository = scope.ServiceProvider.GetRequiredService<IAgendamentoRepository>();

            string assunto = "Novo agendamento recebido";
            string corpo = $"Você tem um novo agendamento em {data.ScheduleDate}.";

            /*AQUI EU DISPARARIA UM E-MAIL PRA CAIXA DE ENTRADA, MAS COMO PRECISARIA DE UMA CONTA SMTP, 
              FAÇO APENAS SALVAR NO BANCO QUE A MENSAGEM FOI ENVIADA PARA O PROFISSIONAL DE SAÚDE REFERENTE AO AGENDAMENTO DELE. */

            // Marcar o email como enviado no banco de dados
            await agendamentoRepository.MarcarEmailComoEnviado(data.Id);

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Erro no ServiceBus: {args.Exception}");
            return Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor != null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
