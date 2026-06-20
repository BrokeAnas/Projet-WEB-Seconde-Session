using System.Text.Json.Serialization;
using MediCareManager.Core.Common;

namespace MediCareManager.Core.Entities;

public class Patient
{
    [JsonConverter(typeof(LongToStringJsonConverter))]
    public long IdNat { get; set; }

    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public DateOnly DateNaissance { get; set; }
    public string? Adresse { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
}
