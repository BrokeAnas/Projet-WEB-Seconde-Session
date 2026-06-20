using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Entities;

public class PaiementHistorique
{
    public int IdHistorique { get; set; }
    public int IdPaiement { get; set; }

    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNatPatient { get; set; }

    public int? IdRdv { get; set; }
    public decimal Montant { get; set; }
    public DateOnly DatePaiement { get; set; }

    /// <summary>UPDATE | DELETE</summary>
    public string Operation { get; set; } = string.Empty;

    public DateTime DateOperation { get; set; }

    // ----- Champ d'affichage (jointure Patient) -----
    public string? PatientNom { get; set; }
}
