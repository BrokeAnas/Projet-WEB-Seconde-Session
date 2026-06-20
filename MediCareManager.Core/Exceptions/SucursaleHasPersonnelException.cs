namespace MediCareManager.Core.Exceptions;

/// <summary>Suppression refusée : du personnel est affecté à la succursale → HTTP 409.</summary>
public class SucursaleHasPersonnelException : Exception
{
    public SucursaleHasPersonnelException(int idSucursale)
        : base($"Impossible de supprimer cette succursale ({idSucursale}) : du personnel y est affecté.") { }
}
