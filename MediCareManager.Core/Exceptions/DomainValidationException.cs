namespace MediCareManager.Core.Exceptions;

/// <summary>
/// Violation d'une règle de validation métier (ex. numéro de Registre National invalide) → HTTP 400.
/// </summary>
public class DomainValidationException : Exception
{
    public DomainValidationException(string message) : base(message) { }
}
