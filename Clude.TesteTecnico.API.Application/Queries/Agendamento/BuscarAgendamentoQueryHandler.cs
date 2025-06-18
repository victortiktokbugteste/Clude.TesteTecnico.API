using Clude.TesteTecnico.API.Application.Queries.Agendamento.Responses;
using Clude.TesteTecnico.API.Application.Queries.Paciente;
using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Queries.Agendamento
{
    public class BuscarAgendamentoQueryHandler : IRequestHandler<BuscarAgendamentoQuery, BuscarAgendamentoResponse>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public BuscarAgendamentoQueryHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<BuscarAgendamentoResponse> Handle(BuscarAgendamentoQuery request, CancellationToken cancellationToken)
        {
            var agendamentoEntity = await _agendamentoRepository.GetByIdAsync(request.Id);

            if (agendamentoEntity == null)
                return null;

            return BuscarAgendamentoResponse.FromDomain(agendamentoEntity);
        }
    }
}
