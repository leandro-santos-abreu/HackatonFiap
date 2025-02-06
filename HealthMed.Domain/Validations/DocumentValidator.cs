using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HealthMed.Domain.Validations;

public static class DocumentValidator
{
    public static bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = cpf.Replace(".", "").Replace("-", "");

        if (!Regex.IsMatch(cpf, @"^\d{11}$"))
            return false;

        return IsCpfValid(cpf);
    }

    private static bool IsCpfValid(string cpf)
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

    public static bool IsValidCRM(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
            return false;

        // Expressão regular para validar UF (2 letras) + número (4 a 6 dígitos)
        if (!Regex.IsMatch(crm, @"^[A-Z]{2}\d{4,6}$"))
            return false;

        return IsCRMValid(crm);
    }

    private static bool IsCRMValid(string crm)
    {

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