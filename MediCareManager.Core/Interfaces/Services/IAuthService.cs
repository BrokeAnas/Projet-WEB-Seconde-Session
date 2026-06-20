namespace MediCareManager.Core.Interfaces.Services;

public interface IAuthService
{
    /// <summary>Retourne le token JWT (incluant rôle/nom/prénom dans les claims) ou null si identifiants invalides.</summary>
    Task<string?> LoginAsync(string email, string password);
}
