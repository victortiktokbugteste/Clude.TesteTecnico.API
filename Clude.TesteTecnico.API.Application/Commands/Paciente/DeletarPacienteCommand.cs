using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;


namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class DeletarPacienteCommand : IRequest<bool>
    {
        [SwaggerSchema(Description = "Id do paciente para deletar")]
        public int Id { get; }

        public DeletarPacienteCommand(int id)
        {
            Id = id;
        }
    }
}
