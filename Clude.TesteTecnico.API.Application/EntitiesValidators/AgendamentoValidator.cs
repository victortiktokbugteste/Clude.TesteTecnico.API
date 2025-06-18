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
            .WithMessage("O ID do paciente deve ser maior que zero");

            RuleFor(x => x.ProfissionalSaudeId)
          .GreaterThan(0)
          .WithMessage("O ID do profissional de saúde deve ser maior que zero");

        }
    }
}
