using MeuProjeto.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Data.DTO;
public class UpdateMedicoDTO
{
    [Key]
    [Required]
    public int IdMedico { get; set; }

    [Required, EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    [Required]
    public string Senha { get; set; }
}
