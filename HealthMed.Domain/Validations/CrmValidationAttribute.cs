using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MeuProjeto.Domain.Validations;
public class CrmValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        string crm = value.ToString().Trim().ToUpper();

        // Expressão regular para validar UF (2 letras) + número (4 a 6 dígitos)
        if (!Regex.IsMatch(crm, @"^[A-Z]{2}\d{4,6}$"))
            return false;

        // Lista de UFs válidas (Brasil)
        string[] ufsValidas = {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT",
            "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO",
            "RR", "SC", "SP", "SE", "TO"
        };

        // Verifica se a UF é válida
        string uf = crm.Substring(0, 2);
        if (!Array.Exists(ufsValidas, u => u == uf))
            return false;

        return true;
    }
}
