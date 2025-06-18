using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;

namespace Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude
{
    public class BuscarTodosProfissionaisSaudeQueryHandler : IRequestHandler<BuscarTodosProfissionaisSaudeQuery, List<BuscarProfissionalSaudeResponse>>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;

        public BuscarTodosProfissionaisSaudeQueryHandler(IProfissionalSaudeRepository profissionalSaudeRepository)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
        }

        public async Task<List<BuscarProfissionalSaudeResponse>> Handle(BuscarTodosProfissionaisSaudeQuery request, CancellationToken cancellationToken)
        {
            var profissionaisSaude = await _profissionalSaudeRepository.GetAllAsync();
            if (profissionaisSaude == null)
                return null;

            return profissionaisSaude.Select(p => BuscarProfissionalSaudeResponse.FromDomain(p)).ToList();
        }
    }
}
