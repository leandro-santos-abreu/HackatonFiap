using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace MeuProjeto.Domain.Validations;

public class CpfValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        var cpf = value.ToString().Replace(".", "").Replace("-", "");

        if (!Regex.IsMatch(cpf, @"^\d{11}$"))
            return false;

        return IsCpfValid(cpf);
    }

    private bool IsCpfValid(string cpf)
    {
        if (cpf.Distinct().Count() == 1)
            return false;

        for (int j = 9; j < 11; j++)
        {
            int sum = 0;
            for (int i = 0; i < j; i++)
                sum += (cpf[i] - '0') * (j + 1 - i);

            int remainder = sum % 11;
            int checkDigit = remainder < 2 ? 0 : 11 - remainder;

            if (cpf[j] - '0' != checkDigit)
                return false;
        }

        return true;
    }
}
