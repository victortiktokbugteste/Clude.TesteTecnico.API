using MediatR;


namespace Clude.TesteTecnico.API.Application.Commands.Paciente
{
    public class DeletarPacienteCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeletarPacienteCommand(int id)
        {
            Id = id;
        }
    }
}
