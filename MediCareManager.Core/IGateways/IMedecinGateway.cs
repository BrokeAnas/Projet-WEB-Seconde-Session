using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface IMedecinGateway
{
    Task<IEnumerable<Medecin>> GetAllAsync(string? search = null);
    Task<Medecin?> GetByIdAsync(long idNat);
    Task<Medecin?> GetByEmailAsync(string email);
    Task<long> CreateAsync(Medecin medecin);
    Task UpdateAsync(Medecin medecin);
    Task DeleteAsync(long idNat);
    Task<bool> HasRendezVousFutursAsync(long idNat);
}
