using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Transactions;
using PacienteEntity = Clude.TesteTecnico.API.Domain.Entities.Paciente;

namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class AtualizaPacienteCommandHandler : IRequestHandler<AtualizaPacienteCommand, AtualizaPacienteResponse>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<PacienteEntity> _validator;

        public AtualizaPacienteCommandHandler(
            IPacienteRepository pacienteRepository,
            IValidator<PacienteEntity> validator)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
        }

        public async Task<AtualizaPacienteResponse> Handle(AtualizaPacienteCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var paciente = new PacienteEntity
                {
                    Name = request.Name,
                    Cpf = request.Cpf,
                    BirthDate = request.BirthDate,
                    Id = request.Id
                };

                var existsPaciente = await _pacienteRepository.GetByIdAsync(request.Id);
                if (existsPaciente == null || existsPaciente.Id == 0)
                {
                    throw new NotFoundException("Paciente não encontrado!");
                }


                var validationResult = await _validator.ValidateAsync(paciente, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }


                await _pacienteRepository.UpdateAsync(paciente);
                scope.Complete();

                return AtualizaPacienteResponse.FromDomain(paciente);
            }
            catch
            {
                throw;
            }
        }
    }
}
