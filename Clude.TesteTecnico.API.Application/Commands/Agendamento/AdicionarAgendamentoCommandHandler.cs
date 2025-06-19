using Clude.TesteTecnico.API.Application.Commands.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AgendamentoEntity = Clude.TesteTecnico.API.Domain.Entities.Agendamento;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento
{
    public class AdicionarAgendamentoCommandHandler : IRequestHandler<AdicionarAgendamentoCommand, AdicionarAgendamentoResponse>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IValidator<AgendamentoEntity> _validator;
        private readonly IMessageBusService _messageBus;
        public AdicionarAgendamentoCommandHandler(IAgendamentoRepository agendamentoRepository,
            IValidator<AgendamentoEntity> validator,
            IMessageBusService messageBus)
        {
            _agendamentoRepository = agendamentoRepository;
            _validator = validator;
            _messageBus = messageBus;
        }

        public async Task<AdicionarAgendamentoResponse> Handle(AdicionarAgendamentoCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var agendamento = new AgendamentoEntity
                {
                    PacienteId = request.PacienteId.GetValueOrDefault(),
                    ProfissionalSaudeId = request.ProfissionalSaudeId.GetValueOrDefault(),
                    CreateDate = DateTime.Now,
                    ScheduleDate = request.ScheduleDate.GetValueOrDefault(),
                    TempoDuracaoAtendimentoMinutos = 30 //Regra definida no escopo do teste técnico. Achei viavél não deixar o front passar essa informação. 
                };

                var validationResult = await _validator.ValidateAsync(agendamento, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                //Preciso validar se o paciente já tem uma consulta com esse mesmo profissional no dia.
                var existsAgendamentoWithScheduleDate = await _agendamentoRepository.ExistsByPacienteAndProfissionalPerDayAsync(agendamento.PacienteId, agendamento.ProfissionalSaudeId, agendamento.ScheduleDate);
                if (existsAgendamentoWithScheduleDate)
                    throw new SingleErrorException("Ja existe um agendamento para esse paciente com esse mesmo profissional nesse mesmo dia.");

                //Preciso validar se o profissional já possui uma consulta naquele horário.
                var agendamentosDoDiaProfissionalSaude = await _agendamentoRepository.GetAgendamentosByProfissionalAndDateAsync(agendamento.ProfissionalSaudeId, agendamento.ScheduleDate);

                // Verifica sobreposição de horários
                var horarioInicio = agendamento.ScheduleDate;
                var horarioFim = horarioInicio.AddMinutes(agendamento.TempoDuracaoAtendimentoMinutos);

                var temSobreposicao = agendamentosDoDiaProfissionalSaude.Any(a => 
                {
                    var inicioExistente = a.ScheduleDate;
                    var fimExistente = inicioExistente.AddMinutes(a.TempoDuracaoAtendimentoMinutos);

                    return (horarioInicio >= inicioExistente && horarioInicio < fimExistente) || 
                           (horarioFim > inicioExistente && horarioFim <= fimExistente) || 
                           (horarioInicio <= inicioExistente && horarioFim >= fimExistente); 
                });

                if (temSobreposicao)
                    throw new SingleErrorException("Ja existe um agendamento para este profissional neste horario.");

                var agendamentoCriado = await _agendamentoRepository.AddAsync(agendamento);

                // Envia para o Service Bus
                await _messageBus.EnviarMensagemAsync(new
                {
                    Id = agendamentoCriado.Id,
                    CreateDate = agendamento.CreateDate,
                    ScheduleDate = request.ScheduleDate,
                    ProfissionalSaudeEmail = request.ProfissionalEmailToReceiveNotification
                });

                return AdicionarAgendamentoResponse.FromDomain(agendamentoCriado);
            }
            catch
            {
                throw;
            }
        }
    }
}
