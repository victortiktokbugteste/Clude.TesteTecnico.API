using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using MediatR;

namespace Clude.TesteTecnico.API.Application.Queries.Paciente
{
    public class BuscarTodosPacientesQuery : IRequest<List<BuscarPacienteResponse>>
    {
    }
}
