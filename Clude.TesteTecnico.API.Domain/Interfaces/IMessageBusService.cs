using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Domain.Interfaces
{
    public interface IMessageBusService
    {
        Task EnviarMensagemAsync(object mensagem);
    }
}
