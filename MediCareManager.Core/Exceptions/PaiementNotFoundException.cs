namespace MediCareManager.Core.Exceptions;

public class PaiementNotFoundException : NotFoundException
{
    public PaiementNotFoundException(int id)
        : base($"Paiement introuvable (id {id}).") { }
}
