using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ProfissionalSaudeEntity = Clude.TesteTecnico.API.Domain.Entities.ProfissionalSaude;

namespace Clude.TesteTecnico.API.Tests.Commands.ProfissionalSaude
{
    public class ExcluirProfissionalSaudeCommandHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly Mock<IAgendamentoRepository> _agendamentoRepository;
        private readonly DeletarProfissionalSaudeCommandHandler _handler;

        public ExcluirProfissionalSaudeCommandHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();
            _agendamentoRepository = new Mock<IAgendamentoRepository>();
            _handler = new DeletarProfissionalSaudeCommandHandler(_profissionalSaudeRepositoryMock.Object, _agendamentoRepository.Object);
        }

        [Fact]
        public async Task Handle_ComProfissionalSaudeExistente_DeveExcluirPaciente()
        {

            var command = new DeletarProfissionalSaudeCommand(1);
            var profissionalSaudeExistente = new ProfissionalSaudeEntity
            {
                Id = command.Id,
                Name = "João Silva",
                Cpf = "12345678900",
                CRM = "CRMTESTE"
            };

            _profissionalSaudeRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(profissionalSaudeExistente);

            var result = await _handler.Handle(command, CancellationToken.None);


            Assert.True(result);
            _profissionalSaudeRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _profissionalSaudeRepositoryMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ComProfissionalSaudeInexistente_DeveLancarNotFoundException()
        {

            var command = new DeletarProfissionalSaudeCommand(1);
            _profissionalSaudeRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((ProfissionalSaudeEntity)null);

            var exception = await Assert.ThrowsAsync<SingleErrorException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Profissional de saúde não encontrado", exception.Message);
        }
    }
} 