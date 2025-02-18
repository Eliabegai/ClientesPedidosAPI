public static class CpfValidator
{
    public static bool isValid(string cpf)
    {
       if(string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        string[] invalidCpfs = new string[]
        {
            "00000000000", "11111111111", "22222222222",
            "33333333333", "44444444444", "55555555555",
            "66666666666", "77777777777", "88888888888",
            "99999999999"
        };

        if (invalidCpfs.Contains(cpf))
            return false;

        var tempCpf = cpf.Substring(0, 9);
        var firstDigit = CalculateDigit(tempCpf);
        tempCpf += firstDigit;
        var secondDigit = CalculateDigit(tempCpf);
        tempCpf += secondDigit;

        return cpf.EndsWith(tempCpf.Substring(9));
    }

    private static int CalculateDigit(string cpf)
    {
        var soma = 0;
        for (int i = 0; i < cpf.Length; i++)
            soma += int.Parse(cpf[i].ToString()) * (cpf.Length + 1 - i);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}