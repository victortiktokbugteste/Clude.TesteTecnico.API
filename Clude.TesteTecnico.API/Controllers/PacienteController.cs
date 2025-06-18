using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Domain.Entities;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Clude.TesteTecnico.API.Application.Queries.Paciente;

namespace Clude.TesteTecnico.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/salvar-paciente")]
        [SwaggerOperation(
        Summary = "Adiciona um paciente",
        Description = "Adiciona um paciente"
        )]
        public async Task<ActionResult<Paciente>> AdicionarPaciente([FromBody] AdicionarPacienteCommand command)
        {            
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpPut("/atualizar-paciente")]
        [SwaggerOperation(
          Summary = "Atualiza um paciente",
          Description = "Atualiza um paciente"
        )]
        public async Task<ActionResult<Paciente>> AtualizaPaciente([FromBody] AtualizaPacienteCommand command)
        {
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpGet("/get-paciente/{id}")]
        [SwaggerOperation(
        Summary = "Busca um paciente pelo ID",
        Description = "Busca um paciente pelo ID"
        )]
        public async Task<ActionResult<Paciente>> BuscarPaciente([FromRoute] int id)
        {
            var paciente = await _mediator.Send(new BuscarPacienteQuery(id));

            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpDelete("/delete-paciente/{id}")]
        [SwaggerOperation(
        Summary = "Deleta um paciente por ID",
        Description = "Remove o paciente da base de dados pelo ID"
        )]
        public async Task<ActionResult> DeletarPaciente([FromRoute] int id)
        {
            var sucesso = await _mediator.Send(new DeletarPacienteCommand(id));
            return sucesso ? NoContent() : NotFound();
        }
    }
}
