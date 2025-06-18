using MediatR;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class AdicionarPacienteCommand : IRequest<AdicionarPacienteResponse>
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }

        public AdicionarPacienteCommand(string name, string cpf, DateTime? birthDate)
        {
            Name = name;
            Cpf = cpf;
            BirthDate = birthDate;
        }
    }
} 