using HealthMed.Domain.Enums;
using MeuProjeto.Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Domain.Entity;

public class PessoaEntity
{

    public virtual EnumTipoPessoa TipoPessoa { get; protected set; }

    [Required(ErrorMessage = "O necessário atribuir o Nome.")]
    [StringLength(500)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "É necessário preencher o CPF")]
    [CpfValidation(ErrorMessage = "CPF inválido.")]
    public string CPF { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }
    [Required]
    public string Senha { get; set; }
}
