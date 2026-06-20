namespace MediCareManager.Core.Exceptions;

/// <summary>Ressource introuvable → HTTP 404.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
