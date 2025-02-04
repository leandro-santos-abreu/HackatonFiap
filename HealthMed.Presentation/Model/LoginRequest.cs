using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HealthMed.Presentation.Model;

public class LoginRequest
{
    [Required(ErrorMessage = "Login obrigatório")]
    public string Login { get; set; }

    [Required]
    [PasswordPropertyText]
    public string Senha { get; set; }
}
