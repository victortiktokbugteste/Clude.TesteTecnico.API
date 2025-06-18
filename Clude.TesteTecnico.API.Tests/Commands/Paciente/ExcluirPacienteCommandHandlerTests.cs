using Xunit;
using Moq;
using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using PacienteEntity = Clude.TesteTecnico.API.Domain.Entities.Paciente;
using Clude.TesteTecnico.API.Application.Exceptions;

namespace Clude.TesteTecnico.API.Tests.Commands.Paciente
{
    public class ExcluirPacienteCommandHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly DeletarPacienteCommandHandler _handler;

        public ExcluirPacienteCommandHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _handler = new DeletarPacienteCommandHandler(_pacienteRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ComPacienteExistente_DeveExcluirPaciente()
        {

            var command = new DeletarPacienteCommand(1);
            var pacienteExistente = new PacienteEntity
            {
                Id = command.Id,
                Name = "João Silva",
                Cpf = "12345678900",
                BirthDate = new System.DateTime(1990, 1, 1)
            };

            _pacienteRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(pacienteExistente);



            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.True(result);
            _pacienteRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _pacienteRepositoryMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ComPacienteInexistente_DeveLancarNotFoundException()
        {

            var command = new DeletarPacienteCommand(1);
            _pacienteRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((PacienteEntity)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Paciente não encontrado", exception.Message);
        }
    }
} 