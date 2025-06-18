using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento
{
    public class DeletarAgendamentoCommandHandler : IRequestHandler<DeletarAgendamentoCommand, bool>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public DeletarAgendamentoCommandHandler(
            IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<bool> Handle(DeletarAgendamentoCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existsAgendamento = await _agendamentoRepository.GetByIdAsync(request.Id);
                if (existsAgendamento == null || existsAgendamento.Id == 0)
                {
                    throw new SingleErrorException("Agendamento não encontrado!");
                }

                await _agendamentoRepository.DeleteAsync(request.Id);
                scope.Complete();

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
