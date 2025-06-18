using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;

namespace Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses
{
    public class BuscarProfissionalSaudeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string CRM { get; set; }

        public static BuscarProfissionalSaudeResponse FromDomain(Domain.Entities.ProfissionalSaude profissionalSaude)
        {
            return new BuscarProfissionalSaudeResponse
            {
                Id = profissionalSaude.Id,
                Name = profissionalSaude.Name,
                Cpf = profissionalSaude.Cpf,
                CRM = profissionalSaude.CRM
            };
        }
    }
} 