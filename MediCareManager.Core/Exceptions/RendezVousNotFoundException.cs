namespace MediCareManager.Core.Exceptions;

public class RendezVousNotFoundException : NotFoundException
{
    public RendezVousNotFoundException(int id)
        : base($"Rendez-vous introuvable (id {id}).") { }
}
