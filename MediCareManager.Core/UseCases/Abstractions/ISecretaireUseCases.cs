using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface ISecretaireUseCases
{
    Task<IEnumerable<Secretaire>> GetAllAsync();
    Task<Secretaire> GetByIdAsync(long idNat);
    Task<long> CreateAsync(CreateSecretaireDto dto);
    Task UpdateAsync(long idNat, UpdateSecretaireDto dto);
    Task DeleteAsync(long idNat);
}
