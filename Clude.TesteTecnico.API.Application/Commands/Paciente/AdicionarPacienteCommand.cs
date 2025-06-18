using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class AdicionarPacienteCommand : IRequest<AdicionarPacienteResponse>
    {
        [SwaggerSchema(Description = "Nome do paciente para cadastro")]
        public string Name { get; set; }

        [SwaggerSchema(Description = "CPF do paciente para cadastro")]
        public string Cpf { get; set; }

        [SwaggerSchema(Description = "Data de Nascimento do paciente para cadastro")]
        public DateTime? BirthDate { get; set; }

        public AdicionarPacienteCommand(string name, string cpf, DateTime? birthDate)
        {
            Name = name;
            Cpf = cpf;
            BirthDate = birthDate;
        }
    }
} 