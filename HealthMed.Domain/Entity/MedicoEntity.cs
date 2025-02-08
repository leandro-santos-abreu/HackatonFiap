﻿using MeuProjeto.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Domain.Entity;
public class MedicoEntity
{
    [Key]
    [Required]
    public int IdMedico { get; set; }
    
    [Required(ErrorMessage ="O necessário atribuir o Nome.")]
    [StringLength(500)]
    public string Nome { get; set; }

    [Required(ErrorMessage ="É necessário preencher o CPF")]
    [CpfValidation(ErrorMessage = "CPF inválido.")]
    public string CPF { get; set; }

    [Required]
    [CrmValidation(ErrorMessage = "CRM inválido. Formato esperado: UF + número (Ex: SP123456).")]
    public string CRM { get; set; }

    [Required(ErrorMessage = "É necessário atribuir uma Especialidade Médica.")]
    public string Especialidade { get; set; }

    [Required , EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
    
    [Required] 
    public string Senha { get; set; }

    public List<AgendaEntity> Agendas { get; set; } = new();
    //public virtual ICollection<AgendaEntity> Agendas { get; set; }
}
