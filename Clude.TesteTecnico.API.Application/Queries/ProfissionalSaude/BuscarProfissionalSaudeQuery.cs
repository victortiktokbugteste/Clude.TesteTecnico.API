using MediatR;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;

namespace Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude
{
    public class BuscarProfissionalSaudeQuery : IRequest<BuscarProfissionalSaudeResponse>
    {
        public int Id { get; set; }

        public BuscarProfissionalSaudeQuery()
        {
        }

        public BuscarProfissionalSaudeQuery(int id)
        {
            Id = id;
        }
    }
} 