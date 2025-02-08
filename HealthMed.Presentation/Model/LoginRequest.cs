using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
