using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Paciente.Responses
{
    public class BuscarPacienteResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }

        public static BuscarPacienteResponse FromDomain(Domain.Entities.Paciente paciente)
        {
            return new BuscarPacienteResponse
            {
                Id = paciente.Id,
                Name = paciente.Name,
                Cpf = paciente.Cpf,
                BirthDate = paciente.BirthDate
            };
        }
    }
}
