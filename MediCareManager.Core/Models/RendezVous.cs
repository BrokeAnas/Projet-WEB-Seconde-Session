using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Models;

public class RendezVous
{
    public int IdRdv { get; set; }

    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNatPatient { get; set; }

    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNatMedecin { get; set; }

    [JsonConverter(typeof(NullableLongToStringJsonConverter))]
    public long? IdNatSecretaire { get; set; }

    public int IdSucursale { get; set; }
    public DateOnly DateRdv { get; set; }
    public TimeOnly HeureDebut { get; set; }
    public TimeOnly HeureFin { get; set; }
    public string? Motif { get; set; }

    /// <summary>Planifié | En cours | Terminé | Annulé</summary>
    public string Statut { get; set; } = "Planifié";

    // ----- Champs d'affichage (jointures Patient / Medecin / Sucursale) -----
    public string? PatientNom { get; set; }
    public string? MedecinNom { get; set; }
    public string? SucursaleNom { get; set; }
}
