using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Commands.Agendamento
{
    public class DeletarAgendamentoCommand : IRequest<bool>
    {
        [SwaggerSchema(Description = "Id do agendamento para deletar")]
        public int Id { get; }

        public DeletarAgendamentoCommand(int id)
        {
            Id = id;
        }
    }
}
