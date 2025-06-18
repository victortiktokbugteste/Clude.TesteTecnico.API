using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Application.EntitiesValidators;
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
    public class AdicionarProfissionalSaudeCommandHandlerTests
    {
        private readonly Mock<IProfissionalSaudeRepository> _profissionalSaudeRepositoryMock;
        private readonly IValidator<ProfissionalSaudeEntity> _validator;
        private readonly AdicionarProfissionalSaudeCommandHandler _handler;

        public AdicionarProfissionalSaudeCommandHandlerTests()
        {
            _profissionalSaudeRepositoryMock = new Mock<IProfissionalSaudeRepository>();
            _validator = new ProfissionalSaudeValidator();
            _handler = new AdicionarProfissionalSaudeCommandHandler(
                _profissionalSaudeRepositoryMock.Object,
                _validator);
        }

        [Theory]
        [InlineData("João Silva", "12345678900", "crmTeste")]
        public async Task Handle_ComDadosValidos_DeveAdicionarProfissionalSaude(string nome, string cpf, string crm)
        {
         
            var command = new AdicionarProfissionalSaudeCommand(nome, cpf, crm);

            var profissionalSaudeCriado = new ProfissionalSaudeEntity
            {
                Id = 1,
                Name = command.Name,
                Cpf = command.Cpf,
                CRM = command.CRM
            };

            _profissionalSaudeRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<ProfissionalSaudeEntity>()))
                .ReturnsAsync(profissionalSaudeCriado);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Cpf, result.Cpf);
            Assert.Equal(command.CRM, result.CRM);

            _profissionalSaudeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProfissionalSaudeEntity>()), Times.Once);
        }

        [Theory]
        [InlineData("", "12345678900", "CRMTESTE", "Nome é obrigatório")]
        [InlineData("João Silva", "", "CRMTESTE", "CPF é obrigatório")]
        [InlineData("João Silva", "12345678900", "", "CRM é obrigatório")]
        public async Task Handle_ComDadosInvalidos_DeveLancarValidationException(string nome, string cpf, string crm, string mensagemEsperada)
        {

            var command = new AdicionarProfissionalSaudeCommand(nome, cpf, crm);

            var profissionalSaudeCriado = new ProfissionalSaudeEntity
            {
                Id = 1,
                Name = command.Name,
                Cpf = command.Cpf,
                CRM = command.CRM
            };

            _profissionalSaudeRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<ProfissionalSaudeEntity>()))
                .ReturnsAsync(profissionalSaudeCriado);

            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Contains(mensagemEsperada, exception.Message);
            _profissionalSaudeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProfissionalSaudeEntity>()), Times.Never);
        }
    }
} 