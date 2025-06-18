using Clude.TesteTecnico.API.Domain.Entities;

namespace Clude.TesteTecnico.API.Domain.Interfaces
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        Task<bool> ExistsByCpfAsync(string cpf, int? id = 0);
    }
} 