namespace MediCareManager.Core.Exceptions;

/// <summary>Suppression refusée : le médecin a des rendez-vous futurs → HTTP 409.</summary>
public class MedecinHasRendezVousException : Exception
{
    public MedecinHasRendezVousException(long idNat)
        : base($"Impossible de supprimer ce médecin ({idNat}) : des rendez-vous futurs sont planifiés.") { }
}
