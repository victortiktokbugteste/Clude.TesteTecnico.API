using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;

namespace Clude.TesteTecnico.API.Application.Queries.Paciente
{
    public class BuscarTodosPacientesQueryHandler : IRequestHandler<BuscarTodosPacientesQuery, List<BuscarPacienteResponse>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public BuscarTodosPacientesQueryHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<List<BuscarPacienteResponse>> Handle(BuscarTodosPacientesQuery request, CancellationToken cancellationToken)
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            if (pacientes == null)
                return null;

            return pacientes.Select(p => BuscarPacienteResponse.FromDomain(p)).ToList();
        }
    }
}
