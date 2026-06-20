namespace MediCareManager.Core.Exceptions;

/// <summary>Numéro national déjà présent (PRIMARY KEY) → HTTP 409.</summary>
public class IdNatAlreadyExistsException : Exception
{
    public IdNatAlreadyExistsException(long idNat)
        : base($"Le numéro national {idNat} existe déjà.") { }
}
