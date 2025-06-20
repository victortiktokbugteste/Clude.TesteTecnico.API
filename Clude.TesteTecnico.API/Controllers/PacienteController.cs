using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Commands.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Queries.Paciente;
using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
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
        [SwaggerResponse(201, "Paciente criado com sucesso", typeof(BuscarPacienteResponse))]
        [SwaggerResponse(400, "Erro ao criar o paciente")]
        public async Task<ActionResult<AdicionarPacienteResponse>> AdicionarPaciente([FromBody] AdicionarPacienteCommand command)
        {            
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpPut("/atualizar-paciente")]
        [SwaggerOperation(
          Summary = "Atualiza um paciente",
          Description = "Atualiza um paciente"
        )]
        [SwaggerResponse(201, "Paciente atualizado com sucesso", typeof(BuscarPacienteResponse))]
        [SwaggerResponse(400, "Erro ao atualizar o paciente")]
        public async Task<ActionResult<AtualizaPacienteResponse>> AtualizaPaciente([FromBody] AtualizaPacienteCommand command)
        {
            var paciente = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpGet("/get-paciente/{id}")]
        [SwaggerOperation(
        Summary = "Busca um paciente pelo ID",
        Description = "Busca um paciente pelo ID"
        )]
        [SwaggerResponse(200, "Paciente encontrado com sucesso", typeof(BuscarPacienteResponse))]
        [SwaggerResponse(404, "Erro ao buscar o paciente")]
        public async Task<ActionResult<BuscarPacienteResponse>> BuscarPaciente([FromRoute] int id)
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
        [SwaggerResponse(204, "Paciente deletado com sucesso")]
        [SwaggerResponse(400, "Erro ao deletar o paciente")]
        public async Task<ActionResult> DeletarPaciente([FromRoute] int id)
        {
            var sucesso = await _mediator.Send(new DeletarPacienteCommand(id));
            return sucesso ? NoContent() : NotFound();
        }

        [HttpGet("/get-todos-pacientes")]
        [SwaggerOperation(
            Summary = "Lista todos os pacientes",
            Description = "Retorna todos os pacientes cadastrados no sistema"
        )]
        [SwaggerResponse(200, "Pacientes encontrados com sucesso", typeof(BuscarPacienteResponse))]
        [SwaggerResponse(400, "Erro ao buscar os pacientes")]
        public async Task<ActionResult<List<BuscarPacienteResponse>>> BuscarTodosPacientes()
        {
            var pacientes = await _mediator.Send(new BuscarTodosPacientesQuery());

            if (pacientes == null || pacientes.Count == 0)
                return NoContent();

            return Ok(pacientes);
        }

    }
}
