namespace MediCareManager.Core.Exceptions;

public class PatientNotFoundException : NotFoundException
{
    public PatientNotFoundException(long idNat)
        : base($"Patient introuvable (numéro national {idNat}).") { }
}
