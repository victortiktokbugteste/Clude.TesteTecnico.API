using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
using Clude.TesteTecnico.API.Application.Exceptions;
using Clude.TesteTecnico.API.Domain.Entities;
using Clude.TesteTecnico.API.Domain.Interfaces;
using FluentValidation;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ProfissionalSaudeEntity = Clude.TesteTecnico.API.Domain.Entities.ProfissionalSaude;

namespace Clude.TesteTecnico.API.Tests.Commands.ProfissionalSaude
{
    public class AtualizarProfissionalSaudeCommandHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly IValidator<ProfissionalSaudeEntity> _validator;
        private readonly AtualizaProfissionalSaudeCommandHandler _handler;

        public AtualizarProfissionalSaudeCommandHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();
            _validator = new ProfissionalSaudeValidator();
            _handler = new AtualizaProfissionalSaudeCommandHandler(
                _profissionalSaudeRepositoryMock.Object,
                _validator);
        }


        [Theory]
        [InlineData("João Silva", "12345678900", "CRMTESTE", 1500)]
        public async Task Handle_ComDadosValidos_DeveAtualizarProfissionalSaude(string nome, string cpf, string crm, int id)
        {

            var command = new AtualizaProfissionalSaudeCommand(nome, cpf, crm, id);
            var profissionalSaudeExistente = new ProfissionalSaudeEntity
            {
                Id = command.Id,
                Name = "Nome Antigo",
                Cpf = "98765432100",
                CRM = "CRM ANTIGO"
            };

            _profissionalSaudeRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(profissionalSaudeExistente);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Cpf, result.Cpf);
            Assert.Equal(command.CRM, result.CRM);

            _profissionalSaudeRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _profissionalSaudeRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ProfissionalSaudeEntity>()), Times.Once);
        }

        [Theory]
        [InlineData("João Silva", "12345678900", "CRMTESTE", 1500)]
        public async Task Handle_ComProfissionalSaudeInexistente_DeveLancarNotFoundException(string nome, string cpf, string crm, int id)
        {

            var command = new AtualizaProfissionalSaudeCommand(nome, cpf, crm, id);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Profissional de saúde não encontrado!", exception.Message);
        }

        [Theory]
        [InlineData("", "12345678900", "CRMTESTE", 1, "Nome é obrigatório")]
        [InlineData("João Silva", "", "CRMTESTE", 1, "CPF é obrigatório")]
        [InlineData("João Silva", "12345678900", "", 1, "CRM é obrigatório")]
        public async Task Handle_ComDadosInvalidos_DeveLancarValidationException(string nome, string cpf, string crm, int id, string mensagemEsperada)
        {

            var command = new AtualizaProfissionalSaudeCommand(nome, cpf, crm, id);

            var profissionalSaude = new ProfissionalSaudeEntity
            {
                Id = id,
                Name = "Registro fake",
                Cpf = "11111",
                CRM = "yrdssadda"
            };

            _profissionalSaudeRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(profissionalSaude);


            var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains(mensagemEsperada, exception.Message);
        }
    }
} 