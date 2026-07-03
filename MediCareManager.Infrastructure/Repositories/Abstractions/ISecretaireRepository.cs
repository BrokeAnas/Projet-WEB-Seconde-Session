using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface ISecretaireRepository
{
    Task<IEnumerable<Secretaire>> GetAllAsync();
    Task<Secretaire?> GetByIdAsync(long idNat);
    Task<Secretaire?> GetByEmailAsync(string email);
    Task<long> CreateAsync(Secretaire secretaire);
    Task UpdateAsync(Secretaire secretaire);
    Task DeleteAsync(long idNat);
}
