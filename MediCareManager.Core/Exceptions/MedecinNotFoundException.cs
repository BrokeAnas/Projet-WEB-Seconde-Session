namespace MediCareManager.Core.Exceptions;

public class MedecinNotFoundException : NotFoundException
{
    public MedecinNotFoundException(long idNat)
        : base($"Médecin introuvable (numéro national {idNat}).") { }
}
