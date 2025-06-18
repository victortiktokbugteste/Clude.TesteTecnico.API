using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Domain.Entities
{
    public class ProfissionalSaude
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string CRM { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
