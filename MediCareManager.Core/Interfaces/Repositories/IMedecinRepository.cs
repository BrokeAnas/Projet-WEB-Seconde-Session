using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface IMedecinRepository
{
    Task<IEnumerable<Medecin>> GetAllAsync(string? search = null);
    Task<Medecin?> GetByIdAsync(long idNat);
    Task<Medecin?> GetByEmailAsync(string email);
    Task<long> CreateAsync(Medecin medecin);
    Task UpdateAsync(Medecin medecin);
    Task DeleteAsync(long idNat);
    Task<bool> HasRendezVousFutursAsync(long idNat);
}
