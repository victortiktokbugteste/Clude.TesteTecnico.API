using Xunit;
using Moq;
using FluentValidation;
using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using PacienteEntity = Clude.TesteTecnico.API.Domain.Entities.Paciente;
using FluentValidation.Results;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;

namespace Clude.TesteTecnico.API.Tests.Commands.Paciente
{
    public class AdicionarPacienteCommandHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly IValidator<PacienteEntity> _validator;
        private readonly AdicionarPacienteCommandHandler _handler;

        public AdicionarPacienteCommandHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _validator = new PacienteValidator();
            _handler = new AdicionarPacienteCommandHandler(
                _pacienteRepositoryMock.Object,
                _validator);
        }

        [Theory]
        [InlineData("João Silva", "12345678900", "1990-01-01")]
        public async Task Handle_ComDadosValidos_DeveAdicionarPaciente(string nome, string cpf, string dataNascimento)
        {
            DateTime? dtNasc = null;
            try
            {
                dtNasc = DateTime.Parse(dataNascimento);
            }
            catch { }

            var command = new AdicionarPacienteCommand(nome, cpf, dtNasc);

            var pacienteCriado = new PacienteEntity
            {
                Id = 1,
                Name = command.Name,
                Cpf = command.Cpf,
                BirthDate = command.BirthDate ?? DateTime.Now
            };

            _pacienteRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<PacienteEntity>()))
                .ReturnsAsync(pacienteCriado);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Cpf, result.Cpf);
            Assert.Equal(command.BirthDate, result.BirthDate);

            _pacienteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<PacienteEntity>()), Times.Once);
        }

        [Theory]
        [InlineData("", "12345678900", "1990-01-01", "Nome é obrigatório")]
        [InlineData("João Silva", "", "1990-01-01", "CPF é obrigatório")]
        [InlineData("João Silva", "12345678900", "", "Data de Nascimento é obrigatório")]
        public async Task Handle_ComDadosInvalidos_DeveLancarValidationException(string nome, string cpf, string dataNascimento, string mensagemEsperada)
        {
            DateTime? dtNasc = null;
            try
            {
                dtNasc = DateTime.Parse(dataNascimento);
            }
            catch { }

            var command = new AdicionarPacienteCommand(nome, cpf, dtNasc);

            var pacienteCriado = new PacienteEntity
            {
                Id = 1,
                Name = command.Name,
                Cpf = command.Cpf,
                BirthDate = command.BirthDate ?? DateTime.Now
            };

            _pacienteRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<PacienteEntity>()))
                .ReturnsAsync(pacienteCriado);

            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains(mensagemEsperada, exception.Message);
            _pacienteRepositoryMock.Verify(r => r.AddAsync(It.IsAny<PacienteEntity>()), Times.Never);
        }
    }
} 