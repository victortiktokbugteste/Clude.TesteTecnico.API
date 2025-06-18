using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class AdicionarProfissionalSaudeCommand : IRequest<AdicionarProfissionalSaudeResponse>
    {
        [SwaggerSchema(Description = "Nome do profissional de saúde para cadastro")]
        public string Name { get; set; }

        [SwaggerSchema(Description = "CPF do profissional de saúde para cadastro")]
        public string Cpf { get; set; }

        [SwaggerSchema(Description = "CRM do profissional de saúde para cadastro")]
        public string CRM { get; set; }

        public AdicionarProfissionalSaudeCommand(string name, string cpf, string crm)
        {
            Name = name;
            Cpf = cpf;
            CRM = crm;
        }
    }
} 