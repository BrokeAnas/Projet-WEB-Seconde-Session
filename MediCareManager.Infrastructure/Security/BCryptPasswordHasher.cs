using MediCareManager.Core.Security;

namespace MediCareManager.Infrastructure.Security;

/// <summary>Implémentation BCrypt de <see cref="IPasswordHasher"/> (work factor 11).</summary>
public class BCryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 11;

    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool Verify(string password, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            // Hash mal formé en base : on considère la vérification comme échouée.
            return false;
        }
    }
}
