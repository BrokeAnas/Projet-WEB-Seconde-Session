using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface ISecretaireGateway
{
    Task<IEnumerable<Secretaire>> GetAllAsync();
    Task<Secretaire?> GetByIdAsync(long idNat);
    Task<Secretaire?> GetByEmailAsync(string email);
    Task<long> CreateAsync(Secretaire secretaire);
    Task UpdateAsync(Secretaire secretaire);
    Task DeleteAsync(long idNat);
}
