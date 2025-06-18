using Clude.TesteTecnico.API.Application.Queries.Paciente.Responses;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude;
using Clude.TesteTecnico.API.Application.Queries.ProfissionalSaude.Responses;
using Clude.TesteTecnico.API.Domain.Interfaces;
using MediatR;

namespace Clude.TesteTecnico.API.Application.Queries.Paciente
{
    public class BuscarPacienteQueryHandler : IRequestHandler<BuscarPacienteQuery, BuscarPacienteResponse>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public BuscarPacienteQueryHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<BuscarPacienteResponse> Handle(BuscarPacienteQuery request, CancellationToken cancellationToken)
        {
            var pacienteEntity = await _pacienteRepository.GetByIdAsync(request.Id);

            if (pacienteEntity == null)
                return null;

            return new BuscarPacienteResponse
            {
                Id = pacienteEntity.Id,
                Name = pacienteEntity.Name,
                Cpf = pacienteEntity.Cpf,
                BirthDate = pacienteEntity.BirthDate
            };
        }
    }
}
