using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses
{
    public class BuscarAgendamentoResponse
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public PacienteResponse Paciente { get; set; }
        public int ProfissionalSaudeId { get; set; }
        public ProfissionalSaudeResponse ProfissionalSaude { get; set; }
        public DateTime CreateDate { get; set; }
        public int TempoDuracaoAtendimentoMinutos { get; set; }
        public DateTime ScheduleDate { get; set; }
        public static BuscarAgendamentoResponse FromDomain(Domain.Entities.Agendamento agendamento)
        {
            return new BuscarAgendamentoResponse
            {
                Id = agendamento.Id,
                PacienteId = agendamento.PacienteId,
                Paciente = new PacienteResponse
                {
                    Name = agendamento.Paciente.Name,
                    Cpf = agendamento.Paciente.Cpf
                },
                ProfissionalSaudeId = agendamento.ProfissionalSaudeId,
                ProfissionalSaude = new ProfissionalSaudeResponse
                {
                    Name = agendamento.ProfissionalSaude.Name,
                    Cpf = agendamento.ProfissionalSaude.Cpf,
                    CRM = agendamento.ProfissionalSaude.CRM
                },
                CreateDate = agendamento.CreateDate,
                TempoDuracaoAtendimentoMinutos = agendamento.TempoDuracaoAtendimentoMinutos,
                ScheduleDate = agendamento.ScheduleDate
            };
        }
    }
}
