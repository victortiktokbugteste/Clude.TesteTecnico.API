using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento.Responses
{
    public class AtualizaAgendamentoResponse
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int ProfissionalSaudeId { get; set; }
        public int TempoDuracaoAtendimentoMinutos { get; set; }
        public DateTime ScheduleDate { get; set; }
        public static AtualizaAgendamentoResponse FromDomain(Domain.Entities.Agendamento agendamento)
        {
            return new AtualizaAgendamentoResponse
            {
                Id = agendamento.Id,
                PacienteId = agendamento.PacienteId,
                ProfissionalSaudeId = agendamento.ProfissionalSaudeId,
                TempoDuracaoAtendimentoMinutos = agendamento.TempoDuracaoAtendimentoMinutos,
                ScheduleDate = agendamento.ScheduleDate
            };
        }
    }
}
