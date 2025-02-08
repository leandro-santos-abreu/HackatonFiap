using System.ComponentModel.DataAnnotations;

namespace HealthMed.Data.DTO
{
    public class UpdateAgendaDTO
    {
        [Key]
        [Required]
        public int IdAgenda { get; set; }
        public DateTime HorarioDisponivel { get; set; }
        public int IdMedico { get; set; }
        public decimal ValorConsulta { get; set; }
    }
}
