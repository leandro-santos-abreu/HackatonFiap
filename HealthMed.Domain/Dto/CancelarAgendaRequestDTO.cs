using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Data.DTO;
public class CancelarAgendaRequestDTO
{    
    public int IdAgenda { get; set; }
    public string JustificativaCancelamento { get; set; }
}
