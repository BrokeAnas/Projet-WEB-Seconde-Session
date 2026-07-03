using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Security;
using MediCareManager.Core.Settings;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

/// <summary>
/// Authentifie l'utilisateur (Médecin → Secrétaire → Administrateur) et génère un JWT signé HS256.
/// Le JWT est construit uniquement avec des API de base .NET (HMACSHA256), ce qui évite toute
/// dépendance externe dans la couche Core.
/// </summary>
public class AuthUseCases : IAuthUseCases
{
    private readonly IMedecinGateway _medecinGateway;
    private readonly ISecretaireGateway _secretaireGateway;
    private readonly IAdministrateurGateway _administrateurGateway;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtSettings _jwt;

    public AuthUseCases(
        IMedecinGateway medecinGateway,
        ISecretaireGateway secretaireGateway,
        IAdministrateurGateway administrateurGateway,
        IPasswordHasher passwordHasher,
        JwtSettings jwt)
    {
        _medecinGateway = medecinGateway;
        _secretaireGateway = secretaireGateway;
        _administrateurGateway = administrateurGateway;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var medecin = await _medecinGateway.GetByEmailAsync(email);
        if (medecin is not null)
        {
            return _passwordHasher.Verify(password, medecin.MotDePasse)
                ? GenerateToken(medecin.IdNat.ToString(), "medecin", medecin.Prenom, medecin.Nom, medecin.IdSucursale)
                : null;
        }

        var secretaire = await _secretaireGateway.GetByEmailAsync(email);
        if (secretaire is not null)
        {
            return _passwordHasher.Verify(password, secretaire.MotDePasse)
                ? GenerateToken(secretaire.IdNat.ToString(), "secretaire", secretaire.Prenom, secretaire.Nom, secretaire.IdSucursale)
                : null;
        }

        var admin = await _administrateurGateway.GetByEmailAsync(email);
        if (admin is not null)
        {
            return _passwordHasher.Verify(password, admin.MotDePasse)
                ? GenerateToken(admin.IdAdmin.ToString(), "admin", admin.Prenom, admin.Nom, null)
                : null;
        }

        // Ne révèle jamais si c'est l'e-mail ou le mot de passe qui est incorrect.
        return null;
    }

    private string GenerateToken(string sub, string role, string prenom, string nom, int? sucursale)
    {
        var now = DateTimeOffset.UtcNow;
        var exp = now.AddHours(_jwt.ExpiryHours);

        var header = new Dictionary<string, object>
        {
            ["alg"] = "HS256",
            ["typ"] = "JWT"
        };

        var payload = new Dictionary<string, object>
        {
            ["sub"] = sub,
            ["role"] = role,
            ["given_name"] = prenom,
            ["family_name"] = nom,
            ["iss"] = _jwt.Issuer,
            ["aud"] = _jwt.Audience,
            ["iat"] = now.ToUnixTimeSeconds(),
            ["nbf"] = now.ToUnixTimeSeconds(),
            ["exp"] = exp.ToUnixTimeSeconds()
        };
        if (sucursale.HasValue) payload["sucursale"] = sucursale.Value;

        var headerSegment = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header));
        var payloadSegment = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));
        var unsignedToken = $"{headerSegment}.{payloadSegment}";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_jwt.Key));
        var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));

        return $"{unsignedToken}.{Base64UrlEncode(signature)}";
    }

    private static string Base64UrlEncode(byte[] bytes)
        => Convert.ToBase64String(bytes)
                  .TrimEnd('=')
                  .Replace('+', '-')
                  .Replace('/', '_');
}
