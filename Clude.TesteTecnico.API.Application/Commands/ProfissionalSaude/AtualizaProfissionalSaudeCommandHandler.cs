using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Transactions;
using ProfissionalSaudeEntity = Clude.TesteTecnico.API.Domain.Entities.ProfissionalSaude;

namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class AtualizaProfissionalSaudeCommandHandler : IRequestHandler<AtualizaProfissionalSaudeCommand, AtualizaProfissionalSaudeResponse>
    {
        private readonly IProfissionalSaudeRepository _profissionalSaudeRepository;
        private readonly IValidator<ProfissionalSaudeEntity> _validator;

        public AtualizaProfissionalSaudeCommandHandler(
            IProfissionalSaudeRepository profissionalSaudeRepository,
            IValidator<ProfissionalSaudeEntity> validator)
        {
            _profissionalSaudeRepository = profissionalSaudeRepository;
            _validator = validator;
        }

        public async Task<AtualizaProfissionalSaudeResponse> Handle(AtualizaProfissionalSaudeCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var existsProfissional = await _profissionalSaudeRepository.GetByIdAsync(request.Id);
                if (existsProfissional == null || existsProfissional.Id == 0)
                {
                    throw new SingleErrorException("Profissional de saúde não encontrado!");
                }

                var profissionalSaude = new ProfissionalSaudeEntity
                {
                    Name = request.Name,
                    Cpf = request.Cpf,
                    CRM = request.CRM,
                    Id = request.Id
                };


                var validationResult = await _validator.ValidateAsync(profissionalSaude, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                //Aqui eu valido se existe outro profissional de saúde que não seja o mesmo e que tenha o mesmo cpf e crm.
                var existsProfissionalSaudeWithSameCpf = await _profissionalSaudeRepository.ExistsByCpfOrCRMAsync(profissionalSaude.Cpf, profissionalSaude.CRM, profissionalSaude.Id);
                if (existsProfissionalSaudeWithSameCpf)
                    throw new SingleErrorException("Já existe um profissional com esse CPF ou CRM.");

                await _profissionalSaudeRepository.UpdateAsync(profissionalSaude);
                scope.Complete();

                return AtualizaProfissionalSaudeResponse.FromDomain(profissionalSaude);
            }
            catch
            {
                throw;
            }
        }
    }
}
