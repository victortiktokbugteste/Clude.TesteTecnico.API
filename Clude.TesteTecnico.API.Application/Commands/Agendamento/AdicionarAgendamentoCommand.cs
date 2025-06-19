using Clude.TesteTecnico.API.Application.Commands.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento
{
    public class AdicionarAgendamentoCommand : IRequest<AdicionarAgendamentoResponse>
    {
        [SwaggerSchema(Description = "ID do paciente para cadastro")]
        public int? PacienteId { get; set; }

        [SwaggerSchema(Description = "ID do profissional para cadastro")]
        public int? ProfissionalSaudeId { get; set; }

        [SwaggerSchema(Description = "Data de Agendamento para cadastro")]
        public DateTime? ScheduleDate { get; set; }

        [SwaggerSchema(Description = "Email do profissional para alertar sobre o agendamento")]
        public string ProfissionalEmailToReceiveNotification { get; set; }

        public AdicionarAgendamentoCommand(int? pacienteId, int? profissionalSaudeId, DateTime? scheduleDate, string profissionalEmailToReceiveNotification)
        {
            PacienteId = pacienteId;
            ProfissionalSaudeId = profissionalSaudeId;
            ScheduleDate = scheduleDate;
            ProfissionalEmailToReceiveNotification = profissionalEmailToReceiveNotification;
        }
    }
}
