using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class AtualizaProfissionalSaudeCommand : IRequest<AtualizaProfissionalSaudeResponse>
    {
        [SwaggerSchema(Description = "Nome do profissional de saúde para atualização")]
        public string Name { get; set; }

        [SwaggerSchema(Description = "CPF do profissional de saúde para atualização")]
        public string Cpf { get; set; }

        [SwaggerSchema(Description = "CRM do profissional de saúde para atualização")]
        public string CRM { get; set; }

        [SwaggerSchema(Description = "Id do profissional de saúde para atualização")]
        public int Id { get; set; }
        public AtualizaProfissionalSaudeCommand(string name, string cpf, string crm, int id)
        {
            Name = name;
            Cpf = cpf;
            CRM = crm;
            Id = id;
        }
    }
}
