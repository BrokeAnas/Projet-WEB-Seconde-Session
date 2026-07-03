using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface IRendezVousUseCases
{
    Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId, long? patientId, int? sucursaleId, DateOnly? date);
    Task<RendezVous> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateRendezVousDto dto); // Lève RendezVousConflitException si conflit
    Task UpdateAsync(int id, UpdateRendezVousDto dto);
    Task UpdateStatutAsync(int id, string statut);
    Task DeleteAsync(int id);
}
