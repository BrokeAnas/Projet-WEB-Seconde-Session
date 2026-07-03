namespace MediCareManager.Core.Models;

public class TypeMaladie
{
    public int IdMaladie { get; set; }
    public string Libelle { get; set; } = string.Empty;

    /// <summary>Code CIM-10 (colonne code_CIM).</summary>
    public string? CodeCim { get; set; }
}
