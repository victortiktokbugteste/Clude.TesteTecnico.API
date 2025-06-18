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
    public class ProfissionalSaudeValidator: AbstractValidator<ProfissionalSaude>
    {
        private static readonly string[] UFs = new[]
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
            "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
            "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };
        public ProfissionalSaudeValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Nome é obrigatório")
           .Matches("^[a-zA-ZÀ-ÿ ]+$").WithMessage("O nome deve conter apenas letras e espaços.");

            RuleFor(x => x.CRM)
           .NotEmpty()
           .WithMessage("CRM é obrigatório")
           .Matches(@"^\d{4,6}[/\-]?[A-Z]{2}$").WithMessage("CRM deve estar no formato 123456/UF. (exemplo: 123456/MG)")
            .Must(crm =>
            {
                var uf = crm[^2..].ToUpper();
                return UFs.Contains(uf);
            }).WithMessage("UF do CRM inválida.");

            RuleFor(x => x.Cpf)
           .NotEmpty()
           .WithMessage("CPF é obrigatório")
           .Must(CpfUtils.EhCpfValido).WithMessage("O CPF informado é inválido."); ;
        }
    }
}
