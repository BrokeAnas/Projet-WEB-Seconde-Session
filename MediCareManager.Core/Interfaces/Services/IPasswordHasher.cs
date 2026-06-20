namespace MediCareManager.Core.Interfaces.Services;

/// <summary>
/// Abstraction du hachage de mot de passe. L'implémentation BCrypt vit dans la couche Infrastructure,
/// ce qui maintient Core libre de toute dépendance externe.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
