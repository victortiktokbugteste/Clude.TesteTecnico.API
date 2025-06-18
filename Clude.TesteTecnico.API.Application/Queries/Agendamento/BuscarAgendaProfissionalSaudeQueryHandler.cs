using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Agendamento
{
    public class BuscarAgendaProfissionalSaudeQueryHandler : IRequestHandler<BuscarAgendaProfissionalSaudeQuery, List<BuscarAgendamentoResponse>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public BuscarAgendaProfissionalSaudeQueryHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<List<BuscarAgendamentoResponse>> Handle(BuscarAgendaProfissionalSaudeQuery request, CancellationToken cancellationToken)
        {
            var agendamentos = await _agendamentoRepository.GetAgendamentosByProfissional(request.Id);
            if (agendamentos == null)
                return null;

            return agendamentos.Select(p => BuscarAgendamentoResponse.FromDomain(p)).ToList();
        }
    }
}
