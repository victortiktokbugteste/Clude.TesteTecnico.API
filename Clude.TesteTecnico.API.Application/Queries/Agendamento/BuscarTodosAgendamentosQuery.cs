using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Agendamento
{
    public class BuscarTodosAgendamentosQuery : IRequest<List<BuscarAgendamentoResponse>>
    {
    }
}
