using Xunit;
using Moq;
using Clude.TesteTecnico.API.Application.Queries.Paciente;
using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using Clude.TesteTecnico.API.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using PacienteEntity = Clude.TesteTecnico.API.Domain.Entities.Paciente;
using Clude.TesteTecnico.API.Application.Exceptions;

namespace Clude.TesteTecnico.API.Tests.Queries.Paciente
{
    public class BuscarPacienteQueryHandlerTests
    {
        private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
        private readonly BuscarPacienteQueryHandler _handler;

        public BuscarPacienteQueryHandlerTests()
        {
            _pacienteRepositoryMock = new Mock<IPacienteRepository>();
            _handler = new BuscarPacienteQueryHandler(_pacienteRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ComPacienteExistente_DeveRetornarPaciente()
        {

            var query = new BuscarPacienteQuery(1);
            var paciente = new PacienteEntity
            {
                Id = query.Id,
                Name = "João Silva",
                Cpf = "12345678900",
                BirthDate = new DateTime(1990, 1, 1)
            };

            _pacienteRepositoryMock
                .Setup(r => r.GetByIdAsync(query.Id))
                .ReturnsAsync(paciente);


            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(paciente.Id, result.Id);
            Assert.Equal(paciente.Name, result.Name);
            Assert.Equal(paciente.Cpf, result.Cpf);
            Assert.Equal(paciente.BirthDate, result.BirthDate);

            _pacienteRepositoryMock.Verify(r => r.GetByIdAsync(query.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ComPacienteInexistente_DeveRetornarNull()
        {

            var query = new BuscarPacienteQuery(1);

            _pacienteRepositoryMock
                .Setup(r => r.GetByIdAsync(query.Id))
                .ReturnsAsync((PacienteEntity)null);


            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(result);
            _pacienteRepositoryMock.Verify(r => r.GetByIdAsync(query.Id), Times.Once);
        }
    }
} 