using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Utils;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.EntitiesValidators
{
    public class AgendamentoValidator : AbstractValidator<Agendamento>
    {
        public AgendamentoValidator()
        {

            RuleFor(x => x.PacienteId)
            .GreaterThan(0)
            .WithMessage("O ID do paciente deve ser maior que zero")
            .When(x=> x.Id == 0);

            RuleFor(x => x.ProfissionalSaudeId)
          .GreaterThan(0)
          .WithMessage("O ID do profissional de saúde deve ser maior que zero")
          .When(x => x.Id == 0);

            RuleFor(x => x.ScheduleDate)
            .GreaterThan(DateTime.Now)
            .WithMessage("A data do agendamento não pode ser menor que a data atual");

            RuleFor(x => x.ScheduleDate)
             .NotEmpty()
             .WithMessage("Data de Agendamento é obrigatória");

            RuleFor(x => x.ScheduleDate.TimeOfDay)
            .GreaterThanOrEqualTo(TimeSpan.FromHours(8))
          .WithMessage("O horário deve ser a partir das 08:00")
          .LessThanOrEqualTo(TimeSpan.FromHours(18))
          .WithMessage("O horário deve ser até às 18:00");

        }
    }
}
