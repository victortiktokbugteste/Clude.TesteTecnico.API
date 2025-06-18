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

        public DeletarProfissionalSaudeCommandHandler(
            IProfissionalSaudeRepository profissionalSaudeRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
        }

        public async Task<bool> Handle(DeletarProfissionalSaudeCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existsProfissionalSaude = await _profissionalSaudeRepository.GetByIdAsync(request.Id);
                if (existsProfissionalSaude == null || existsProfissionalSaude.Id == 0)
                {
                    throw new NotFoundException("Profissional de saúde não encontrado!");
                }

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
