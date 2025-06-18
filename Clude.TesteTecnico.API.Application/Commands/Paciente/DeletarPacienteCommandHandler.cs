using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class DeletarPacienteCommandHandler : IRequestHandler<DeletarPacienteCommand, bool>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public DeletarPacienteCommandHandler(
            IPacienteRepository pacienteRepository, 
            IAgendamentoRepository agendamentoRepository)
        {
            _pacienteRepository = pacienteRepository;
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<bool> Handle(DeletarPacienteCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existsPaciente = await _pacienteRepository.GetByIdAsync(request.Id);
                if (existsPaciente == null || existsPaciente.Id == 0)
                {
                    throw new SingleErrorException("Paciente não encontrado!");
                }

                //Vou deletar as consultas ligadas ao paciente antes de fazer a exclusão dele.
                await _agendamentoRepository.DeletarConsultasDoPaciente(request.Id);

                await _pacienteRepository.DeleteAsync(request.Id);
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
