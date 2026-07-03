using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface IRendezVousRepository
{
    Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId = null, long? patientId = null, int? sucursaleId = null, DateOnly? date = null);
    Task<RendezVous?> GetByIdAsync(int id);
    Task<bool> HasConflitAsync(long medecinId, DateOnly date, TimeOnly heureDebut, TimeOnly heureFin, int? excludeId = null);
    Task<int> CreateAsync(RendezVous rdv);
    Task UpdateAsync(RendezVous rdv);
    Task DeleteAsync(int id);
}
