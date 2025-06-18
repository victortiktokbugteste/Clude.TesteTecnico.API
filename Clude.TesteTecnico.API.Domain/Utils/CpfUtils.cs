using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clude.TesteTecnico.API.Domain.Utils
{
    public static class CpfUtils
    {
        public static bool EhCpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || cpf.All(d => d == cpf[0]))
                return false;

            var cpfNumeros = cpf.Select(c => int.Parse(c.ToString())).ToArray();

            for (int j = 9; j <= 10; j++)
            {
                int soma = 0;
                for (int i = 0; i < j; i++)
                    soma += cpfNumeros[i] * (j + 1 - i);

                int digitoVerificador = (soma * 10) % 11;
                if (digitoVerificador == 10) digitoVerificador = 0;

                if (cpfNumeros[j] != digitoVerificador)
                    return false;
            }

            return true;
        }
    }
}
