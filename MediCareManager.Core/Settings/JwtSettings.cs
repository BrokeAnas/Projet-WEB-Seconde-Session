namespace MediCareManager.Core.Settings;

/// <summary>
/// Paramètres de signature et de validation du JWT.
/// Renseignés depuis appsettings.json côté API (section "Jwt") et injectés dans Core.
/// </summary>
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryHours { get; set; } = 8;
}
