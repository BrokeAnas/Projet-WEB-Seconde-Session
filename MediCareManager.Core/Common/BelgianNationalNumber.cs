namespace MediCareManager.Core.Common;

/// <summary>
/// Validation du numéro de Registre National belge (NISS) via le contrôle modulo 97.
/// Les 9 premiers chiffres forment N, les 2 derniers le checksum CC.
///   - Naissances avant 2000 : valide si (97 - (N % 97)) == CC
///   - Naissances après 2000 : idem avec N précédé d'un « 2 »
/// </summary>
public static class BelgianNationalNumber
{
    public static bool IsValid(string? idNat)
    {
        if (string.IsNullOrWhiteSpace(idNat)) return false;

        var digits = new string(idNat.Where(char.IsDigit).ToArray());
        if (digits.Length != 11) return false;

        var nine = digits.Substring(0, 9);
        if (!int.TryParse(digits.Substring(9, 2), out var cc)) return false;

        var n = long.Parse(nine);
        if (97 - (int)(n % 97) == cc) return true;          // né avant 2000

        var n2 = long.Parse("2" + nine);
        return 97 - (int)(n2 % 97) == cc;                   // né après 2000
    }

    /// <summary>Retourne la valeur numérique (11 chiffres) du numéro national.</summary>
    public static long ToLong(string idNat)
        => long.Parse(new string(idNat.Where(char.IsDigit).ToArray()));
}
