using MeuProjeto.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Data.DTO;

public class UpdatePacienteDTO
{
    [Key]
    [Required]
    public int IdPaciente { get; set; }

    [Required, EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
    [Required]
    public string Senha { get; set; }
}
