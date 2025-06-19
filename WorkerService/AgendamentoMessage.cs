using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService
{
    public class AgendamentoMessage
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ProfissionalSaudeEmail { get; set; }
    }
}
