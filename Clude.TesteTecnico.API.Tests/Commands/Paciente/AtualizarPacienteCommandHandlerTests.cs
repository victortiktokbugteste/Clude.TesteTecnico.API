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
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;

namespace Clude.TesteTecnico.API.Tests.Commands.Paciente
{
    public class AtualizarPacienteCommandHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly IValidator<PacienteEntity> _validator;
        private readonly AtualizaPacienteCommandHandler _handler;

        public AtualizarPacienteCommandHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _validator = new PacienteValidator();
            _handler = new AtualizaPacienteCommandHandler(
                _pacienteRepositoryMock.Object,
                _validator);
        }

        [Theory]
        [InlineData("João Silva", "12345678900", "1990-01-01", 1500)]
        public async Task Handle_ComDadosValidos_DeveAtualizarPaciente(string nome, string cpf, string dataNascimento, int id)
        {

            var command = new AtualizaPacienteCommand(nome, cpf, DateTime.Parse(dataNascimento), id);
            var pacienteExistente = new PacienteEntity
            {
                Id = command.Id,
                Name = "Nome Antigo",
                Cpf = "98765432100",
                BirthDate = new DateTime(1980, 1, 1)
            };

            _pacienteRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(pacienteExistente);


            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Cpf, result.Cpf);
            Assert.Equal(command.BirthDate, result.BirthDate);

            _pacienteRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _pacienteRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<PacienteEntity>()), Times.Once);
        }

        [Theory]
        [InlineData("João Silva", "12345678900", "1990-01-01", 1500)]
        public async Task Handle_ComPacienteInexistente_DeveLancarNotFoundException(string nome, string cpf, string dataNascimento, int id)
        {

            var command = new AtualizaPacienteCommand(nome, cpf, DateTime.Parse(dataNascimento), id);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Paciente não encontrado!", exception.Message);
        }

        [Theory]
        [InlineData("", "12345678900", "1990-01-01", 1, "Nome é obrigatório")]
        [InlineData("João Silva", "", "1990-01-01", 1, "CPF é obrigatório")]
        [InlineData("João Silva", "12345678900", "", 1, "Data de Nascimento é obrigatório")]
        public async Task Handle_ComDadosInvalidos_DeveLancarValidationException(string nome, string cpf, string dataNascimento, int id, string mensagemEsperada)
        {
            DateTime? dtNasc = null;
            try
            {
                dtNasc = DateTime.Parse(dataNascimento);
            }
            catch { }

            var command = new AtualizaPacienteCommand(nome, cpf, dtNasc, id);

            var paciente = new PacienteEntity
            {
                Id = id,
                Name = "Registro fake",
                Cpf = "11111",
                BirthDate = new DateTime(1990, 1, 1)
            };

            _pacienteRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(paciente);


            var exception = await Assert.ThrowsAnyAsync<Exception>(() => 
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains(mensagemEsperada, exception.Message);
        }
    }
} 