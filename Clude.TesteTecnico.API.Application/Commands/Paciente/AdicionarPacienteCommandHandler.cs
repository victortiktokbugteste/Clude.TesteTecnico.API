using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Transactions;
using PacienteEntity = Clude.TesteTecnico.API.Domain.Entities.Paciente;

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

                var validationResult = await _validator.ValidateAsync(paciente, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var existsPacienteWithSameCpf = await _pacienteRepository.ExistsByCpfAsync(paciente.Cpf);
                if (existsPacienteWithSameCpf)
                    throw new SingleErrorException("Já existe um paciente com esse CPF.");


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