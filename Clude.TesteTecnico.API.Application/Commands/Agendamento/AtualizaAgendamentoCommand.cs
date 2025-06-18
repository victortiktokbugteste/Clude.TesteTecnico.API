using Clude.TesteTecnico.API.Application.Commands.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento
{
    public class AtualizaAgendamentoCommand : IRequest<AtualizaAgendamentoResponse>
    {

        [SwaggerSchema(Description = "Id do atendimento para atualização")]
        public int Id { get; set; }
        [SwaggerSchema(Description = "ID do paciente para cadastro")]
        public int PacienteId { get; set; }

        [SwaggerSchema(Description = "ID do profissional para cadastro")]
        public int ProfissionalSaudeId { get; set; }
        [SwaggerSchema(Description = "Data de Agendamento para atualização")]
        public DateTime? ScheduleDate { get; set; }
        public AtualizaAgendamentoCommand(int id, int pacienteId, int profissionalSaudeId, DateTime? scheduleDate)
        {
            Id = id;
            ScheduleDate = scheduleDate;
            PacienteId = pacienteId;
            ProfissionalSaudeId = profissionalSaudeId;
        }
    }
}
