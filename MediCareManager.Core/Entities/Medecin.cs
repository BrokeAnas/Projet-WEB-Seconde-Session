using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Entities;

public class Medecin
{
    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNat { get; set; }

    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    /// <summary>Hash BCrypt. Jamais renvoyé au client.</summary>
    [JsonIgnore]
    public string MotDePasse { get; set; } = string.Empty;

    public int IdSpecialisation { get; set; }
    public int? IdSucursale { get; set; }

    // ----- Champs d'affichage (peuplés par jointures côté Infrastructure) -----
    public string? Specialisation { get; set; }
    public string? Sucursale { get; set; }
}
