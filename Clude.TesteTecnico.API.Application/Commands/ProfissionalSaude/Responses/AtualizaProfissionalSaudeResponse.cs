using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses
{
    public class AtualizaProfissionalSaudeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string CRM { get; set; }
        public DateTime CreateDate { get; set; }

        public static AtualizaProfissionalSaudeResponse FromDomain(Domain.Entities.ProfissionalSaude profissionalSaude)
        {
            return new AtualizaProfissionalSaudeResponse
            {
                Id = profissionalSaude.Id,
                Name = profissionalSaude.Name,
                Cpf = profissionalSaude.Cpf,
                CRM = profissionalSaude.CRM,
                CreateDate = profissionalSaude.CreateDate
            };
        }
    }
}
