using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
using MediatR;


namespace Clude.TesteTecnico.API.Application.Queries.Agendamento
{
    public class BuscarAgendamentoQuery : IRequest<BuscarAgendamentoResponse>
    {
        public int Id { get; set; }

        public BuscarAgendamentoQuery(int id)
        {
            Id = id;
        }
    }
}
