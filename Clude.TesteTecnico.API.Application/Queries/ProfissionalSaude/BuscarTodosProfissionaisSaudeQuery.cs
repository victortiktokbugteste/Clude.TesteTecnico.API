using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using MediatR;
namespace Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude
{
    public class BuscarTodosProfissionaisSaudeQuery : IRequest<List<BuscarProfissionalSaudeResponse>>
    {
    }
}
