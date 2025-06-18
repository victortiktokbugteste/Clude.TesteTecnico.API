using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;


namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class DeletarProfissionalSaudeCommand : IRequest<bool>
    {
        [SwaggerSchema(Description = "Id do profissional de saúde para deletar")]
        public int Id { get; }

        public DeletarProfissionalSaudeCommand(int id)
        {
            Id = id;
        }
    }
}
