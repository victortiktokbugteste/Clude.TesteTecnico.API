using MediatR;


namespace Clude.TesteTecnico.API.Application.Commands.ProfissionalSaude
{
    public class DeletarProfissionalSaudeCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeletarProfissionalSaudeCommand(int id)
        {
            Id = id;
        }
    }
}
