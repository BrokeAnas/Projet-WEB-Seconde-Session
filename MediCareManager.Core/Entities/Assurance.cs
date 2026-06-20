namespace MediCareManager.Core.Entities;

public class Assurance
{
    public int IdAssurance { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? Type { get; set; }
}
