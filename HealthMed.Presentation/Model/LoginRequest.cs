using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MeuProjeto.Domain.Validations;

namespace HealthMed.Presentation.Model;

public class LoginRequest
{
    
    public string CPF { get; set; }
   
    public string CRM { get; set; }

    public string Login { get; set; }

    [Required]
    [PasswordPropertyText]
    public string Senha { get; set; }
}
