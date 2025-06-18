using MediatR;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class AdicionarProfissionalSaudeCommand : IRequest<AdicionarProfissionalSaudeResponse>
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string CRM { get; set; }

        public AdicionarProfissionalSaudeCommand(string name, string cpf, string crm)
        {
            Name = name;
            Cpf = cpf;
            CRM = crm;
        }
    }
} 