using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Data.DTO
{
    public class UpdateAgendaDTO
    {
        [Key]
        [Required]
        public int IdAgenda { get; set; }
        public DateTime HorarioDisponivel { get; set; }
        public int IdMedico { get; set; }
    }
}
