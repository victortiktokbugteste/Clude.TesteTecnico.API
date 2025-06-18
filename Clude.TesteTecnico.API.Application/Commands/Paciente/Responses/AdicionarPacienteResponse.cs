namespace Clude.TesteTecnico.API.Application.Commands.Paciente.Responses
{
    public class AdicionarPacienteResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreateDate { get; set; }

        public static AdicionarPacienteResponse FromDomain(Domain.Entities.Paciente paciente)
        {
            return new AdicionarPacienteResponse
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