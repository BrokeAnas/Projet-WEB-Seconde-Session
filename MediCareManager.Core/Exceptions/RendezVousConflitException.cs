namespace MediCareManager.Core.Exceptions;

/// <summary>Conflit d'agenda (chevauchement d'horaires) → HTTP 409.</summary>
public class RendezVousConflitException : Exception
{
    public RendezVousConflitException(long medecinId, DateOnly date, TimeOnly heureDebut, TimeOnly heureFin)
        : base($"Ce médecin a déjà un rendez-vous le {date:dd/MM/yyyy} entre {heureDebut:HH\\:mm} et {heureFin:HH\\:mm}.") { }
}
