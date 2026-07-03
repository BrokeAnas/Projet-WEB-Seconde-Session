using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface IRendezVousUseCases
{
    Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId, DateOnly? date);
    Task<RendezVous> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateRendezVousDto dto); // Lève RendezVousConflitException si conflit
    Task UpdateStatutAsync(int id, string statut);
}
