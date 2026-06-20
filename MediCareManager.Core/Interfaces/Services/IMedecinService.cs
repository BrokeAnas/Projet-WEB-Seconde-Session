using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Services;

public interface IMedecinService
{
    Task<IEnumerable<Medecin>> GetAllAsync(string? search);
    Task<Medecin> GetByIdAsync(long idNat);
    Task<long> CreateAsync(CreateMedecinDto dto);
    Task UpdateAsync(long idNat, UpdateMedecinDto dto);
    Task DeleteAsync(long idNat); // Lève MedecinHasRendezVousException si RDV futurs
}
