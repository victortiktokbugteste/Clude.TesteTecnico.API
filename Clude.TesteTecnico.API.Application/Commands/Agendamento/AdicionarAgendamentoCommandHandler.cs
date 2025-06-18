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
    public class AdicionarAgendamentoCommandHandler : IRequestHandler<AdicionarAgendamentoCommand, AdicionarAgendamentoResponse>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IValidator<AgendamentoEntity> _validator;
        public AdicionarAgendamentoCommandHandler(IAgendamentoRepository agendamentoRepository,
            IValidator<AgendamentoEntity> validator)
        {
            _agendamentoRepository = agendamentoRepository;
            _validator = validator;
        }

        public async Task<AdicionarAgendamentoResponse> Handle(AdicionarAgendamentoCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var agendamento = new AgendamentoEntity
                {
                    PacienteId = request.PacienteId,
                    ProfissionalSaudeId = request.ProfissionalSaudeId,
                    CreateDate = DateTime.Now,
                    ScheduleDate = request.ScheduleDate,
                    TempoDuracaoAtendimentoMinutos = 30 //Regra definida no escopo do teste técnico. Achei viavél não deixar o front passar essa informação. 
                };

                var validationResult = await _validator.ValidateAsync(agendamento, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var agendamentoCriado = await _agendamentoRepository.AddAsync(agendamento);
                scope.Complete();

                return AdicionarAgendamentoResponse.FromDomain(agendamentoCriado);
            }
            catch
            {
                throw;
            }
        }
    }
}
