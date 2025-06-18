using Clude.TesteTecnico.API.Domain.Entities;

namespace Clude.TesteTecnico.API.Domain.Interfaces
{
    public interface IProfissionalSaudeRepository : IRepository<ProfissionalSaude>
    {
        Task<bool> ExistsByCpfOrCRMAsync(string cpf, string crm, int? id = 0);
    }
} 