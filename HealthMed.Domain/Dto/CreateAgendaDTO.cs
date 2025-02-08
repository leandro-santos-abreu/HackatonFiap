using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Data.DTO
{
    public class CreateAgendaDTO
    {
        public DateTime HorarioDisponivel { get; set; }
        public int IdMedico { get; set; }
        public decimal ValorConsulta { get; set; }
    }
}
