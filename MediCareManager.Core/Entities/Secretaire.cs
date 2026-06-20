using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Entities;

public class Secretaire
{
    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNat { get; set; }

    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    public string MotDePasse { get; set; } = string.Empty;

    public int IdSucursale { get; set; }

    // ----- Champ d'affichage -----
    public string? Sucursale { get; set; }
}
