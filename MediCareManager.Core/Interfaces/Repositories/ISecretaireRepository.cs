using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface ISecretaireRepository
{
    Task<IEnumerable<Secretaire>> GetAllAsync();
    Task<Secretaire?> GetByIdAsync(long idNat);
    Task<Secretaire?> GetByEmailAsync(string email);
    Task<long> CreateAsync(Secretaire secretaire);
    Task UpdateAsync(Secretaire secretaire);
    Task DeleteAsync(long idNat);
}
