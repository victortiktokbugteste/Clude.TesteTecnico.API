using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Agendamento
{
    public class BuscarAgendaProfissionalSaudeQuery : IRequest<List<BuscarAgendamentoResponse>>
    {
        public int Id { get; set; }

        public BuscarAgendaProfissionalSaudeQuery(int id)
        {
            Id = id;
        }
    }
}
