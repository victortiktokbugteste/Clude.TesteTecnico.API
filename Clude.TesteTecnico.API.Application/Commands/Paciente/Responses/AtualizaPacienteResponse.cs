using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente.Responses
{
    public class AtualizaPacienteResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreateDate { get; set; }

        public static AtualizaPacienteResponse FromDomain(Domain.Entities.Paciente paciente)
        {
            return new AtualizaPacienteResponse
            {
                Id = paciente.Id,
                Name = paciente.Name,
                Cpf = paciente.Cpf,
                BirthDate = paciente.BirthDate,
                CreateDate = paciente.CreateDate
            };
        }
    }
}
