using Clude.TesteTecnico.API.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.EntitiesValidators
{
    public class ProfissionalSaudeValidator: AbstractValidator<ProfissionalSaude>
    {
        public ProfissionalSaudeValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Nome é obrigatório");

            RuleFor(x => x.CRM)
           .NotEmpty()
           .WithMessage("CRM é obrigatório");

            RuleFor(x => x.Cpf)
           .NotEmpty()
           .WithMessage("CPF é obrigatório");
        }
    }
}
