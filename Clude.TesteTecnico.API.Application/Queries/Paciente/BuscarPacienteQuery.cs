using MediatR;
using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;

namespace Clude.TesteTecnico.API.Application.Queries.Paciente
{
    public class BuscarPacienteQuery : IRequest<BuscarPacienteResponse>
    {
        public int Id { get; set; }

        public BuscarPacienteQuery(int id)
        {
            Id = id;
        }
    }
} 