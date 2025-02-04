using System.ComponentModel.DataAnnotations;

namespace HealthMed.Domain.Entity;

public class AgendaEntity
{
    [Key]
    [Required]
    public int IdAgenda { get; set; }
    public DateTime HorarioDisponivel { get; set; }
    public bool isHorarioMarcado { get; set; } = false;
    public bool isMedicoNotificado { get; set; } = false;
        
    public int IdMedico { get; set; }
   public virtual MedicoEntity Medico { get; set; }
}
