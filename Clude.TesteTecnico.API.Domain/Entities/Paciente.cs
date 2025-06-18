using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Domain.Entities
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
