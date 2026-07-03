using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Models;

/// <summary>Médecin en lecture seule : sert aux listes déroulantes de l'agenda.</summary>
public class Medecin
{
    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNat { get; set; }

    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;

    // ----- Champs d'affichage (peuplés par jointures côté Infrastructure) -----
    public string? Specialisation { get; set; }
    public string? Sucursale { get; set; }
}
