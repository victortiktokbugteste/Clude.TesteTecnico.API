using MediatR;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using PacienteEntity = Clude.TesteTecnico.API.Domain.Entities.Paciente;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using FluentValidation;
using System.Transactions;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class AdicionarPacienteCommandHandler : IRequestHandler<AdicionarPacienteCommand, AdicionarPacienteResponse>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<PacienteEntity> _validator;

        public AdicionarPacienteCommandHandler(
            IPacienteRepository pacienteRepository,
            IValidator<PacienteEntity> validator)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
        }

        public async Task<AdicionarPacienteResponse> Handle(AdicionarPacienteCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var paciente = new PacienteEntity
                {
                    Name = request.Name,
                    Cpf = request.Cpf,
                    BirthDate = request.BirthDate
                };

                // Realiza a validação
                var validationResult = await _validator.ValidateAsync(paciente, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var pacienteCriado = await _pacienteRepository.AddAsync(paciente);
                scope.Complete();

                return AdicionarPacienteResponse.FromDomain(pacienteCriado);
            }
            catch
            {
                throw;
            }
        }
    }
} 