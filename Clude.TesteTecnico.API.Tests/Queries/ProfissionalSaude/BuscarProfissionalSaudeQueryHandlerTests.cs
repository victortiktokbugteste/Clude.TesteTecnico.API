using Clude.TesteTecnico.API.Application.Queries.Paciente;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ProfissionalSaudeEntity = Clude.TesteTecnico.API.Domain.Entities.ProfissionalSaude;

namespace Clude.TesteTecnico.API.Tests.Queries.ProfissionalSaude
{
    public class BuscarProfissionalSaudeQueryHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly BuscarProfissionalSaudeQueryHandler _handler;

        public BuscarProfissionalSaudeQueryHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();
            _handler = new BuscarProfissionalSaudeQueryHandler(_profissionalSaudeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ComProfissionalSaudeExistente_DeveRetornarProfissionalSaude()
        {

            var query = new BuscarProfissionalSaudeQuery(1);
            var paciente = new ProfissionalSaudeEntity
            {
                Id = query.Id,
                Name = "João Silva",
                Cpf = "12345678900",
                CRM = "testeCRM"
            };

            _profissionalSaudeRepositoryMock
                .Setup(r => r.GetByIdAsync(query.Id))
                .ReturnsAsync(paciente);


            var result = await _handler.Handle(query, CancellationToken.None);


            Assert.NotNull(result);
            Assert.Equal(paciente.Id, result.Id);
            Assert.Equal(paciente.Name, result.Name);
            Assert.Equal(paciente.Cpf, result.Cpf);
            Assert.Equal(paciente.CRM, result.CRM);

            _profissionalSaudeRepositoryMock.Verify(r => r.GetByIdAsync(query.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ComProfissionalSaudeInexistente_DeveRetornarNull()
        {

            var query = new BuscarProfissionalSaudeQuery(1);

            _profissionalSaudeRepositoryMock
                .Setup(r => r.GetByIdAsync(query.Id))
                .ReturnsAsync((ProfissionalSaudeEntity)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(result);
            _profissionalSaudeRepositoryMock.Verify(r => r.GetByIdAsync(query.Id), Times.Once);
        }
    }
} 