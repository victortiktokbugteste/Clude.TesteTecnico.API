using MediatR;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;

namespace Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude
{
    public class BuscarProfissionalSaudeQueryHandler : IRequestHandler<BuscarProfissionalSaudeQuery, BuscarProfissionalSaudeResponse>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;

        public BuscarProfissionalSaudeQueryHandler(IProfissionalSaudeRepository profissionalSaudeRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
        }

        public async Task<BuscarProfissionalSaudeResponse> Handle(BuscarProfissionalSaudeQuery request, CancellationToken cancellationToken)
        {
            var profissionalEntity = await _profissionalSaudeRepository.GetByIdAsync(request.Id);
            
            if (profissionalEntity == null)
                return null;

            return new BuscarProfissionalSaudeResponse
            {
                Id = profissionalEntity.Id,
                Name = profissionalEntity.Name,
                Cpf = profissionalEntity.Cpf,
                CRM = profissionalEntity.CRM
            };
        }
    }
} 