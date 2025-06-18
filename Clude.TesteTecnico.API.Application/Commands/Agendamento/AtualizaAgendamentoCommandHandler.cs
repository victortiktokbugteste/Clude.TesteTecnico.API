using Clude.TesteTecnico.API.Application.Commands.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Exceptions;
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
    public class AtualizaAgendamentoCommandHandler : IRequestHandler<AtualizaAgendamentoCommand, AtualizaAgendamentoResponse>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IValidator<AgendamentoEntity> _validator;

        public AtualizaAgendamentoCommandHandler(IAgendamentoRepository agendamentoRepository, IValidator<AgendamentoEntity> validator)
        {
            _agendamentoRepository = agendamentoRepository;
            _validator = validator;
        }

        public async Task<AtualizaAgendamentoResponse> Handle(AtualizaAgendamentoCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existsAgendamento = await _agendamentoRepository.GetByIdAsync(request.Id);
                if (existsAgendamento == null || existsAgendamento.Id == 0)
                {
                    throw new SingleErrorException("Agendamento não encontrado!");
                }

                //Aqui eu deixo ele trocar apenas o dia/horário do agendamento dele. Caso haja disponibilidade
                var agendamento = new AgendamentoEntity
                {
                    ScheduleDate = request.ScheduleDate.GetValueOrDefault(),
                    Id = request.Id
                };

                var validationResult = await _validator.ValidateAsync(agendamento, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                //Preciso validar se o paciente já tem uma consulta com esse mesmo profissional no dia.
                var existsAgendamentoWithScheduleDate = await _agendamentoRepository.ExistsByPacienteAndProfissionalPerDayAsync(request.PacienteId, request.ProfissionalSaudeId, agendamento.ScheduleDate, agendamento.Id);
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
                    throw new SingleErrorException("Já existe um agendamento para este profissional neste horário.");

                await _agendamentoRepository.UpdateAsync(agendamento);
                scope.Complete();

                return AtualizaAgendamentoResponse.FromDomain(agendamento);
            }
            catch
            {
                throw;
            }
        }
    }
}
