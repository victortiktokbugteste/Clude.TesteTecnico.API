using Clude.TesteTecnico.API.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Application.Interfaces
{
    public interface ILogService
    {
        Task RegistrarAsync(LogDto log);
    }
}
