using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Transactions;
using ProfissionalSaudeEntity = Clude.TesteTecnico.API.Domain.Entities.ProfissionalSaude;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class AdicionarProfissionalSaudeCommandHandler : IRequestHandler<AdicionarProfissionalSaudeCommand, AdicionarProfissionalSaudeResponse>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;
        private readonly IValidator<ProfissionalSaudeEntity> _validator;

        public AdicionarProfissionalSaudeCommandHandler(
            IProfissionalSaudeRepository profissionalSaudeRepository,
            IValidator<ProfissionalSaudeEntity> validator)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
            _validator = validator;
        }

        public async Task<AdicionarProfissionalSaudeResponse> Handle(AdicionarProfissionalSaudeCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var profissionalSaude = new ProfissionalSaudeEntity
                {
                    Name = request.Name,
                    Cpf = request.Cpf,
                    CRM = request.CRM
                };

                var validationResult = await _validator.ValidateAsync(profissionalSaude, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var existsProfissionalSaudeWithSameCpf = await _profissionalSaudeRepository.ExistsByCpfOrCRMAsync(profissionalSaude.Cpf, profissionalSaude.CRM);
                if (existsProfissionalSaudeWithSameCpf)
                    throw new SingleErrorException("Já existe um profissional com esse CPF ou CRM.");


                var profissionalCriado = await _profissionalSaudeRepository.AddAsync(profissionalSaude);
                scope.Complete();

                return AdicionarProfissionalSaudeResponse.FromDomain(profissionalCriado);
            }
            catch
            {
                throw;
            }
        }
    }
} 