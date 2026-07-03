using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface IMedecinUseCases
{
    Task<IEnumerable<Medecin>> GetAllAsync(string? search);
    Task<Medecin> GetByIdAsync(long idNat);
    Task<long> CreateAsync(CreateMedecinDto dto);
    Task UpdateAsync(long idNat, UpdateMedecinDto dto);
    Task DeleteAsync(long idNat); // Lève MedecinHasRendezVousException si RDV futurs
}
