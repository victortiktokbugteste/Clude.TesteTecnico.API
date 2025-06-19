using WorkerService;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);

// Registrar o repositório
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
