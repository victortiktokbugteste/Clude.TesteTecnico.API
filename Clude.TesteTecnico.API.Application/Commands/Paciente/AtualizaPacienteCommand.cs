using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class AtualizaPacienteCommand : IRequest<AtualizaPacienteResponse>
    {
        [SwaggerSchema(Description = "Nome do paciente para atualização")]
        public string Name { get; set; }

        [SwaggerSchema(Description = "CPF do paciente para atualização")]
        public string Cpf { get; set; }

        [SwaggerSchema(Description = "Data de Nascimento do paciente para atualização")]
        public DateTime? BirthDate { get; set; }

        [SwaggerSchema(Description = "Id do paciente para atualização")]
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
