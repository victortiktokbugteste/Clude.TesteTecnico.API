using Clude.TesteTecnico.API.Domain.Entities;

namespace Clude.TesteTecnico.API.Domain.Interfaces
{
    public interface IAgendamentoRepository : IRepository<Agendamento>
    {
        Task<List<Agendamento>> GetAgendamentosByProfissional(int profissionalId);
        Task<bool> ExistsByPacienteAndProfissionalPerDayAsync(int pacienteId, int profissionalId, DateTime scheduleDate, int? id = 0);
        Task<List<Agendamento>> GetAgendamentosByProfissionalAndDateAsync(int profissionalId, DateTime scheduleDate);

        Task<bool> DeletarConsultasDoPaciente(int pacienteId);
        Task<bool> DeletarConsultasDoProfissionalDeSaude(int profissionalSaudeId);
    }
}
