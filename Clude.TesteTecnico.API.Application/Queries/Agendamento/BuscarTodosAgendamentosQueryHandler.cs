using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Agendamento
{
    public class BuscarTodosAgendamentosQueryHandler : IRequestHandler<BuscarTodosAgendamentosQuery, List<BuscarAgendamentoResponse>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public BuscarTodosAgendamentosQueryHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<List<BuscarAgendamentoResponse>> Handle(BuscarTodosAgendamentosQuery request, CancellationToken cancellationToken)
        {
            var agendamentos = await _agendamentoRepository.GetAllAsync();
            if (agendamentos == null)
                return null;

            return agendamentos.Select(p => BuscarAgendamentoResponse.FromDomain(p)).ToList();
        }
    }
}
