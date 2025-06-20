using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Application.Queries.Paciente;
using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

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
        [SwaggerResponse(201, "Profissional de saúde criado com sucesso", typeof(BuscarProfissionalSaudeResponse))]
        [SwaggerResponse(400, "Erro ao criar o profissional de saúde")]
        public async Task<ActionResult<AdicionarProfissionalSaudeResponse>> AdicionarProfissionalSaude([FromBody] AdicionarProfissionalSaudeCommand command)
        {
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarProfissionalSaude), new { id = paciente.Id }, paciente);
        }

        [HttpPut("/atualizar-profissional-saude")]
        [SwaggerOperation(
          Summary = "Atualiza um profissional de saúde",
          Description = "Atualiza um profissional de saúde"
        )]
        [SwaggerResponse(201, "Profissional de saúde atualizado com sucesso", typeof(BuscarProfissionalSaudeResponse))]
        [SwaggerResponse(400, "Erro ao atualizar o profissional de saúde")]
        public async Task<ActionResult<AtualizaProfissionalSaudeResponse>> AtualizaProfissionalSaude([FromBody] AtualizaProfissionalSaudeCommand command)
        {
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarProfissionalSaude), new { id = paciente.Id }, paciente);
        }

        [HttpGet("/get-profissional-saude/{id}")]
        [SwaggerOperation(
        Summary = "Busca um profissional de saúde por ID",
        Description = "Busca o profissional de saúde da base de dados pelo ID"
        )]
        [SwaggerResponse(200, "Profissional de saúde encontrado com sucesso", typeof(BuscarProfissionalSaudeResponse))]
        [SwaggerResponse(404, "Erro ao buscar o profissional de saúde")]
        public async Task<ActionResult<BuscarProfissionalSaudeResponse>> BuscarProfissionalSaude([FromRoute] int id)
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
        [SwaggerResponse(204, "Profissional de saúde deletado com sucesso")]
        [SwaggerResponse(400, "Erro ao deletar o profissional de saúde")]
        public async Task<ActionResult> DeletarProfissionalSaude([FromRoute] int id)
        {
            var sucesso = await _mediator.Send(new DeletarProfissionalSaudeCommand(id));
            return sucesso ? NoContent() : NotFound();
        }

        [HttpGet("/get-todos-profissionais-saude")]
        [SwaggerOperation(
        Summary = "Lista todos os profissionais de saúde",
        Description = "Retorna todos os profissionais de saúde cadastrados no sistema"
        )]
        [SwaggerResponse(200, "Profissionais de saúde encontrados com sucesso", typeof(BuscarProfissionalSaudeResponse))]
        [SwaggerResponse(400, "Erro ao buscar os profissionais de saúde")]
        public async Task<ActionResult<List<BuscarProfissionalSaudeResponse>>> BuscarTodosProfissionaisSaude()
        {
            var profissionaisSaude = await _mediator.Send(new BuscarTodosProfissionaisSaudeQuery());

            if (profissionaisSaude == null || profissionaisSaude.Count == 0)
                return NoContent();

            return Ok(profissionaisSaude);
        }
    }
}
