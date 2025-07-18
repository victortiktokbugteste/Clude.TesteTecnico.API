﻿using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Utils;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.EntitiesValidators
{
    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Nome é obrigatório")
           .Matches("^[a-zA-ZÀ-ÿ ]+$").WithMessage("O nome deve conter apenas letras e espaços.");

            RuleFor(x => x.Cpf)
           .NotEmpty()
           .WithMessage("CPF é obrigatório")
           .Must(CpfUtils.EhCpfValido).WithMessage("O CPF informado é inválido.");

            RuleFor(x => x.BirthDate)
           .NotEmpty()
           .WithMessage("Data de Nascimento é obrigatório");

        }
    }
}
