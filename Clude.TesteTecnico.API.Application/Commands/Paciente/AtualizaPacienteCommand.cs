using MediatR;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class AtualizaPacienteCommand : IRequest<AtualizaPacienteResponse>
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Id { get; set; }
        public AtualizaPacienteCommand(string name, string cpf, DateTime? birthDate, int id)
        {
            Name = name;
            Cpf = cpf;
            BirthDate = birthDate;
            Id = id;
        }
    }
}
