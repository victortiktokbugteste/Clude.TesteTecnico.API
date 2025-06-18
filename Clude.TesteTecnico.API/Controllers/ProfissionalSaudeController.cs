using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Clude.TesteTecnico.API.Domain.Entities;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.Paciente;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;

namespace Clude.TesteTecnico.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionalSaudeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfissionalSaudeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/salvar-profissional-saude")]
        [SwaggerOperation(
        Summary = "Adiciona um profissional de saúde",
        Description = "Adiciona um profissional de saúde"
        )]
        public async Task<ActionResult<ProfissionalSaude>> AdicionarProfissionalSaude([FromBody] AdicionarProfissionalSaudeCommand command)
        {
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarProfissionalSaude), new { id = paciente.Id }, paciente);
        }

        [HttpPut("/atualizar-profissional-saude")]
        [SwaggerOperation(
          Summary = "Atualiza um profissional de saúde",
          Description = "Atualiza um profissional de saúde"
        )]
        public async Task<ActionResult<ProfissionalSaude>> AtualizaProfissionalSaude([FromBody] AtualizaProfissionalSaudeCommand command)
        {
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarProfissionalSaude), new { id = paciente.Id }, paciente);
        }

        [HttpGet("/get-profissional-saude/{id}")]
        [SwaggerOperation(
        Summary = "Busca um profissional de saúde por ID",
        Description = "Busca o profissional de saúde da base de dados pelo ID"
        )]
        public async Task<ActionResult<ProfissionalSaude>> BuscarProfissionalSaude([FromRoute] int id)
        {
            var paciente = await _mediator.Send(new BuscarProfissionalSaudeQuery(id));

            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpDelete("/delete-profissional-saude/{id}")]
        [SwaggerOperation(
        Summary = "Deleta um profissional de saúde por ID",
        Description = "Remove o profissional de saúde da base de dados pelo ID"
        )]
        public async Task<ActionResult> DeletarProfissionalSaude([FromRoute] int id)
        {
            var sucesso = await _mediator.Send(new DeletarProfissionalSaudeCommand(id));
            return sucesso ? NoContent() : NotFound();
        }
    }
}
