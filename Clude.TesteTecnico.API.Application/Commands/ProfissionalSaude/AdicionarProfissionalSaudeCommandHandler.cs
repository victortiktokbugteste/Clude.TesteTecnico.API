using MediatR;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using ProfissionalSaudeEntity = Clude.TesteTecnico.API.Domain.Entities.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using FluentValidation;
using System.Transactions;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;

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

                // Realiza a validação
                var validationResult = await _validator.ValidateAsync(profissionalSaude, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

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