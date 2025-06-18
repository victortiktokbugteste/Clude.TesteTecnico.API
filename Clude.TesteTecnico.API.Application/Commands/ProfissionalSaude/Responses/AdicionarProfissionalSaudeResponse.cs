using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses
{
    public class AdicionarProfissionalSaudeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string CRM { get; set; }
        public DateTime CreateDate { get; set; }

        public static AdicionarProfissionalSaudeResponse FromDomain(Domain.Entities.ProfissionalSaude profissionalSaude)
        {
            return new AdicionarProfissionalSaudeResponse
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
