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
                //Aqui eu deixo ele trocar apenas o dia do agendamento dele. Caso haja disponibilidade
                var agendamento = new AgendamentoEntity
                {
                    ScheduleDate = request.ScheduleDate,
                    Id = request.Id
                };

                var existsAgendamento = await _agendamentoRepository.GetByIdAsync(request.Id);
                if (existsAgendamento == null || existsAgendamento.Id == 0)
                {
                    throw new SingleErrorException("Agendamento não encontrado!");
                }


                var validationResult = await _validator.ValidateAsync(agendamento, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

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
