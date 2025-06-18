using Clude.TesteTecnico.API.Application.Commands.Agendamento;
using Clude.TesteTecnico.API.Application.Commands.Paciente;
using Clude.TesteTecnico.API.Application.Queries.Agendamento;
using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgendaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/salvar-agendamento")]
        [SwaggerOperation(
          Summary = "Adiciona um agendamento",
          Description = "Adiciona um agendamento"
          )]
        [SwaggerResponse(201, "Agendamento criado com sucesso", typeof(BuscarAgendamentoResponse))]
        [SwaggerResponse(400, "Erro ao criar o agendamento")]
        public async Task<ActionResult<Agendamento>> AdicionarAgendamento([FromBody] AdicionarAgendamentoCommand command)
        {
            var agendamento = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarAgendamento), new { id = agendamento.Id }, agendamento);
        }

        [HttpPut("/atualizar-agendamento")]
        [SwaggerOperation(
        Summary = "Atualiza um agendamento",
        Description = "Atualiza um agendamento"
      )]
        [SwaggerResponse(201, "Agendamento atualizado com sucesso", typeof(BuscarAgendamentoResponse))]
        [SwaggerResponse(400, "Erro ao atualizar o agendamento")]
        public async Task<ActionResult<Agendamento>> AtualizaPaciente([FromBody] AtualizaAgendamentoCommand command)
        {
            var agendamento = await _mediator.Send(command);
            return CreatedAtAction(nameof(BuscarAgendamento), new { id = agendamento.Id }, agendamento);
        }

        [HttpGet("/get-agendamento/{id}")]
        [SwaggerOperation(
           Summary = "Busca um agendamento pelo ID",
           Description = "Busca um agendamento pelo ID"
           )]
        [SwaggerResponse(200, "Agendamento encontrado com sucesso", typeof(BuscarAgendamentoResponse))]
        [SwaggerResponse(404, "Erro ao buscar o agendamento")]
        public async Task<ActionResult<Agendamento>> BuscarAgendamento([FromRoute] int id)
        {
            var agendamento = await _mediator.Send(new BuscarAgendamentoQuery(id));

            if (agendamento == null)
                return NotFound();

            return Ok(agendamento);
        }

        [HttpDelete("/delete-agendamento/{id}")]
        [SwaggerOperation(
           Summary = "Deleta um agendamento por ID",
           Description = "Remove o agendamento da base de dados pelo ID"
           )]
        [SwaggerResponse(204, "Agendamento deletado com sucesso")]
        [SwaggerResponse(400, "Erro ao deletar o agendamento")]
        public async Task<ActionResult> DeletarAgendamento([FromRoute] int id)
        {
            var sucesso = await _mediator.Send(new DeletarAgendamentoCommand(id));
            return sucesso ? NoContent() : NotFound();
        }

        [HttpGet("/get-todos-agendamentos")]
        [SwaggerOperation(
         Summary = "Lista todos os agendamentos",
         Description = "Retorna todos os agendamentos cadastrados no sistema"
     )]
        [SwaggerResponse(200, "Agendamentos encontrados com sucesso", typeof(BuscarAgendamentoResponse))]
        [SwaggerResponse(400, "Erro ao buscar os agendamentos")]
        public async Task<ActionResult<List<Agendamento>>> BuscarTodosAgendamentos()
        {
            var agendamentos = await _mediator.Send(new BuscarTodosAgendamentosQuery());

            if (agendamentos == null || agendamentos.Count == 0)
                return NoContent();

            return Ok(agendamentos);
        }

    }
}
