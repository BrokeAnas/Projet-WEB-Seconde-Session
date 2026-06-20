namespace MediCareManager.Core.Exceptions;

/// <summary>Adresse e-mail déjà utilisée (contrainte UNIQUE) → HTTP 409.</summary>
public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException(string email)
        : base($"L'adresse e-mail « {email} » est déjà utilisée.") { }
}
