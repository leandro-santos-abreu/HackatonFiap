using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthMed.Domain.Entity;

public class AgendaEntity
{
    [Key]
    [Required]
    public int IdAgenda { get; set; }
    public DateTime HorarioDisponivel { get; set; }
    public bool isHorarioMarcado { get; set; } = false;
    public bool isMedicoNotificado { get; set; } = false;
    public bool isConfirmacaoMedico { get; set; } = false;
    public string? JustificativaCancelamento { get; set; }

    [ForeignKey("Medico")]
    public int IdMedico { get; set; }
    public MedicoEntity Medico { get; set; }

    [ForeignKey("Paciente")]
    public int? IdPaciente { get; set; }  // Pode ser null se o horário estiver livre
    public PacienteEntity? Paciente { get; set; }
    public decimal ValorConsulta { get; set; }
}
