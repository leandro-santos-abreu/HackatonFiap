﻿using MeuProjeto.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Data.DTO;
public class CreateMedicoDTO
{
    [Required(ErrorMessage = "O necessário atribuir o Nome.")]
    [StringLength(500)]
    public string Nome { get; set; }

    [Required(ErrorMessage ="É necessário preencher o CPF")]
    [CpfValidation(ErrorMessage = "CPF inválido.")]
    public string CPF { get; set; }

    [Required]
    [CrmValidation(ErrorMessage = "CRM inválido. Formato esperado: UF + número (Ex: SP123456).")]
    public string CRM { get; set; }
    
    [Required , EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
    
    [Required] 
    public string Senha { get; set; }
}
