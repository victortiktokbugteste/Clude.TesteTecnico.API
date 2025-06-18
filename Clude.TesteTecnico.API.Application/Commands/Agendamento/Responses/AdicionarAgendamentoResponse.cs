using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento.Responses
{
    public class AdicionarAgendamentoResponse
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int ProfissionalSaudeId { get; set; }
        public DateTime CreateDate { get; set; }
        public int TempoDuracaoAtendimentoMinutos { get; set; }
        public DateTime ScheduleDate { get; set; }

        public static AdicionarAgendamentoResponse FromDomain(Domain.Entities.Agendamento agendamento)
        {
            return new AdicionarAgendamentoResponse
            {
                Id = agendamento.Id,
                PacienteId = agendamento.PacienteId,
                ProfissionalSaudeId = agendamento.ProfissionalSaudeId,
                CreateDate = agendamento.CreateDate,
                TempoDuracaoAtendimentoMinutos = agendamento.TempoDuracaoAtendimentoMinutos,
                ScheduleDate = agendamento.ScheduleDate
            };
        }
    }
}
