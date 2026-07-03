using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface IRendezVousRepository
{
    Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId = null, DateOnly? date = null);
    Task<RendezVous?> GetByIdAsync(int id);
    Task<bool> HasConflitAsync(long medecinId, DateOnly date, TimeOnly heureDebut, TimeOnly heureFin);
    Task<int> CreateAsync(RendezVous rdv);
    Task UpdateStatutAsync(int id, string statut);
}
