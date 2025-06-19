using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Domain.Entities
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public int ProfissionalSaudeId { get; set; }
        public ProfissionalSaude ProfissionalSaude { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int TempoDuracaoAtendimentoMinutos { get; set; }
        public bool EmailEnviadoPeloServiceBus { get; set; }
    }
}
