using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class DeletarProfissionalSaudeCommandHandler : IRequestHandler<DeletarProfissionalSaudeCommand, bool>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public DeletarProfissionalSaudeCommandHandler(
            IProfissionalSaudeRepository profissionalSaudeRepository, 
            IAgendamentoRepository agendamentoRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<bool> Handle(DeletarProfissionalSaudeCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existsProfissionalSaude = await _profissionalSaudeRepository.GetByIdAsync(request.Id);
                if (existsProfissionalSaude == null || existsProfissionalSaude.Id == 0)
                {
                    throw new SingleErrorException("Profissional de saúde não encontrado!");
                }

                //Vou deletar as consultas ligadas ao profissional de saúde antes de fazer a exclusão dele.
                await _agendamentoRepository.DeletarConsultasDoProfissionalDeSaude(request.Id);

                await _profissionalSaudeRepository.DeleteAsync(request.Id);
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
