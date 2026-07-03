using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Models;

public class PatientAssurance
{
    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNatPatient { get; set; }

    public int IdAssurance { get; set; }
    public string? NumeroAffiliation { get; set; }
    public DateOnly? DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }

    // ----- Champs d'affichage (jointure Assurance) -----
    public string? Nom { get; set; }
    public string? Type { get; set; }
}
