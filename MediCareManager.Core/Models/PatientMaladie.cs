using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Models;

public class PatientMaladie
{
    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNatPatient { get; set; }

    public int IdMaladie { get; set; }

    [JsonConverter(typeof(NullableLongToStringJsonConverter))]
    public long? IdNatMedecin { get; set; }

    public DateOnly DateDiagnostic { get; set; }
    public string? Observations { get; set; }

    // ----- Champs d'affichage (jointures TypeMaladie / Medecin) -----
    public string? Libelle { get; set; }
    public string? CodeCim { get; set; }
    public string? MedecinNom { get; set; }
}
