namespace MediCareManager.Core.Entities;

public class Sucursale
{
    public int IdSucursale { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public string? Email { get; set; }
}
