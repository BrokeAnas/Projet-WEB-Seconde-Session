using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Models;

public class Paiement
{
    public int IdPaiement { get; set; }

    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNatPatient { get; set; }

    public int? IdRdv { get; set; }
    public decimal Montant { get; set; }
    public DateOnly DatePaiement { get; set; }
    public string? ModePaiement { get; set; }

    // ----- Champ d'affichage (jointure Patient) -----
    public string? PatientNom { get; set; }
}
