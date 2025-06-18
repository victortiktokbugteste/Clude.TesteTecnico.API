using MediatR;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class AtualizaProfissionalSaudeCommand : IRequest<AtualizaProfissionalSaudeResponse>
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string CRM { get; set; }
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
